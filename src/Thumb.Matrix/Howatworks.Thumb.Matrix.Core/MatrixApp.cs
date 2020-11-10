using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using Howatworks.Thumb.Core;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixApp));

        private readonly IConfiguration _config;
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

        private BehaviorSubject<DateTimeOffset> _updateSubject = new BehaviorSubject<DateTimeOffset>(DateTimeOffset.MinValue);

        public IObservable<DateTimeOffset> Updates => _updateSubject.AsObservable();

        public MatrixApp(
            IConfiguration config,
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

        private Uri BuildLocationUri(string cmdrName, string gameVersion)
        {
            return new Uri($"Api/{cmdrName}/{gameVersion}/Location", UriKind.Relative);
        }

        private Uri BuildSessionUri(string cmdrName, string gameVersion)
        {
            return new Uri($"Api/{cmdrName}/{gameVersion}/Session", UriKind.Relative);
        }

        private Uri BuildShipUri(string cmdrName, string gameVersion)
        {
            return new Uri($"Api/{cmdrName}/{gameVersion}/Ship", UriKind.Relative);
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

            var startTime = DateTimeOffset.MinValue; // TODO: temporary
            var source = new JournalEntrySource(_parser, startTime, _logMonitor, _liveMonitor);

            var publisher = new JournalEntryPublisher(source);
            var publication = publisher.GetObservable().Publish();

            _gameContext.SubscribeTo(publication);
            _location.SubscribeTo(publication);
            _ship.SubscribeTo(publication);
            _session.SubscribeTo(publication);

            _logMonitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.File.FullName}'");
            _logMonitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.File.FullName}'");

            var itemsPushed = 0;
            _location.Observable
                .Subscribe(l =>
                {
                    var uri = BuildLocationUri(_gameContext.CommanderName, _gameContext.GameVersion);
                    _client.Push(uri, l);
                    itemsPushed++;
                });

            _ship.Observable
                .Subscribe(s =>
                {
                    var uri = BuildShipUri(_gameContext.CommanderName, _gameContext.GameVersion);
                    _client.Push(uri, s);
                    itemsPushed++;
                });

            _session.Observable
                .Subscribe(s =>
                {
                    var uri = BuildSessionUri(_gameContext.CommanderName, _gameContext.GameVersion);
                    _client.Push(uri, s);
                    itemsPushed++;
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
                        _updateSubject.OnNext(t);
                    }, async ex =>
                    {
                        if (ex is MatrixAuthenticationException)
                        {
                            Log.Warn(ex.Message);
                            // Block further processing until we at least attempt authentication
                            try
                            {
                                var authenticated = await _authenticator.RequestAuthentication();
                            }
                            catch (MatrixException mex)
                            {
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
                        Log.Debug($"Total items pushed = {itemsPushed}");
                        readyForNextBatch.Set();
                    }, token);

                    // Wait until either no longer blocked, or the app is being shut down
                    WaitHandle.WaitAny(new[] { readyForNextBatch.WaitHandle, token.WaitHandle });
                }
            }
        }
    }
}