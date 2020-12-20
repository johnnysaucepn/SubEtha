using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using Howatworks.Thumb.Core;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Matrix.Core
{
    public class MatrixApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixApp));

        private readonly IConfiguration _config;
        private readonly IJournalMonitorState _state;
        private readonly LogJournalMonitor _logMonitor;
        private readonly LiveJournalMonitor _liveMonitor;
        private readonly IThumbNotifier _notifier;
        private readonly IJournalParser _parser;
        private readonly IMatrixAuthenticator _authenticator;

        private readonly GameContextManager _gameContext;
        private readonly LocationManager _location;
        private readonly ShipManager _ship;
        private readonly SessionManager _session;
        private readonly HttpUploadClient _client;

        private readonly Subject<DateTimeOffset> _updateSubject = new Subject<DateTimeOffset>();
        public IObservable<Timestamped<DateTimeOffset>> Updates => _updateSubject
                                                                    .Throttle(TimeSpan.FromSeconds(5))
                                                                    .Timestamp()
                                                                    .AsObservable();

        public MatrixApp(
            IConfiguration config,
            IJournalMonitorState state,
            LogJournalMonitor logMonitor,
            LiveJournalMonitor liveMonitor,
            IThumbNotifier notifier,
            IJournalParser parser,
            IMatrixAuthenticator authenticator,

            GameContextManager gameContext,
            LocationManager location,
            ShipManager ship,
            SessionManager session,

            HttpUploadClient client
        )
        {
            _config = config;
            _state = state;
            _logMonitor = logMonitor;
            _liveMonitor = liveMonitor;
            _notifier = notifier;
            _parser = parser;
            _authenticator = authenticator;

            _gameContext = gameContext;
            _location = location;
            _ship = ship;
            _session = session;

            _client = client;
        }

        public void Run(CancellationToken token)
        {
            Log.Info("Starting up");

            var username = _config["Username"];
            var password = _config["Password"];

            // Try username and password from configuration, if possible
            try
            {
                var nowAuthenticated = _client.Authenticate(username, password);
            }
            catch (MatrixException ex)
            {
                Log.Warn(ex.Message);
            }

            var startTime = _state.LastEntrySeen ?? DateTimeOffset.MinValue;
            var source = new JournalEntrySource(_parser, startTime, _logMonitor, _liveMonitor);

            var publisher = new JournalEntryPublisher(source);
            var publication = publisher.Observable.Publish();

            _gameContext.SubscribeTo(publication);
            _location.SubscribeTo(publication);
            _ship.SubscribeTo(publication);
            _session.SubscribeTo(publication);

            _logMonitor.JournalFileWatch.Subscribe(
                e =>
                {
                    switch (e.Action)
                    {
                        case JournalWatchAction.Started:
                            _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{e.File.FullName}'");
                            break;
                        case JournalWatchAction.Stopped:
                            _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{e.File.FullName}'");
                            break;
                    }
                });

                _location.Observable
                .Subscribe(l =>
                {
                    if (_location.TryBuildUri(_gameContext.CommanderName, _gameContext.GameVersion, out var uri))
                    {
                        _notifier.Notify(NotificationPriority.Low, NotificationEventType.Update, $"Pushed {nameof(LocationState)} update");
                        _client.Push(uri, l);
                    }
                });

            _ship.Observable
                .Subscribe(s =>
                {
                    if (_ship.TryBuildUri(_gameContext.CommanderName, _gameContext.GameVersion, out var uri))
                    {
                        _notifier.Notify(NotificationPriority.Low, NotificationEventType.Update, $"Pushed {nameof(ShipState)} update");
                        _client.Push(uri, s);
                    }
                });

            _session.Observable
                .Subscribe(s =>
                {
                    if (_session.TryBuildUri(_gameContext.CommanderName, _gameContext.GameVersion, out var uri))
                    {
                        _notifier.Notify(NotificationPriority.Low, NotificationEventType.Update, $"Pushed {nameof(SessionState)} update");
                        _client.Push(uri, s);
                    }
                });

            Updates
                .Subscribe(x =>
                {
                    var lastEntry = x.Value;
                    var lastChecked = x.Timestamp;
                    _state.Update(lastChecked, lastEntry);
                    _notifier.Notify(NotificationPriority.Low, NotificationEventType.JournalEntry, "Journal entries applied");
                });

            var readyForNextBatch = new ManualResetEventSlim(true);

            using (publication.Connect())
            {
                while (!token.IsCancellationRequested)
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();

                    publisher.Poll();

                    readyForNextBatch.Reset();

                    _client.StartUploading(token).Subscribe(t =>
                    {
                        _notifier.Notify(NotificationPriority.Low, NotificationEventType.Update, "Uploaded data");
                        _updateSubject.OnNext(t);
                    }, async ex =>
                    {
                        if (ex is MatrixAuthenticationException)
                        {
                            _notifier.Notify(NotificationPriority.High, NotificationEventType.Error, "Authentication failure");
                            Log.Warn(ex.Message);
                            // Block further processing until we at least attempt authentication
                            try
                            {
                                var authenticated = await _authenticator.RequestAuthentication();
                            }
                            catch (MatrixException mex)
                            {
                                _notifier.Notify(NotificationPriority.Medium, NotificationEventType.Error, "Authentication failure");
                                Log.Warn(mex.Message);
                            }
                        }
                        else
                        {
                            Log.Error(ex.Message);
                        }
                        readyForNextBatch.Set();
                    }, () =>
                    {
                        // Upload was successful, can proceed with another poll
                        Log.Debug("Completed pending uploads");
                        readyForNextBatch.Set();
                    }, token);

                    // Wait until either no longer blocked, or the app is being shut down
                    WaitHandle.WaitAny(new[] { readyForNextBatch.WaitHandle, token.WaitHandle });
                }
            }
        }
    }
}