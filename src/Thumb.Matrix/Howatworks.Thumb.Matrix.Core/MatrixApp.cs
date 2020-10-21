using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using Howatworks.Thumb.Core;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixApp : IThumbApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixApp));

        private DateTimeOffset? _lastChecked = null;
        private DateTimeOffset? _lastEntry = null;
        private DateTimeOffset? _lastUpload = null;

        private readonly IConfiguration _config;
        private readonly LogJournalMonitor _logMonitor;
        private readonly LiveJournalMonitor _liveMonitor;
        private readonly IThumbNotifier _notifier;
        private readonly IJournalParser _parser;

        private readonly GameContextManager _gameContext;
        private readonly LocationManager _location;
        private readonly ShipManager _ship;
        private readonly SessionManager _session;
        private readonly HttpUploadClient _client;

        public MatrixApp(
            IConfiguration config,
            LogJournalMonitor logMonitor,
            LiveJournalMonitor liveMonitor,
            IThumbNotifier notifier,
            IJournalParser parser,

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

            _gameContext = gameContext;
            _location = location;
            _ship = ship;
            _session = session;

            _client = client;
        }

        public event EventHandler OnAuthenticationReceived;

        public event EventHandler OnAuthenticationRequired;

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
            var nowAuthenticated = _client.Authenticate(username, password);

            var startTime = DateTimeOffset.MinValue; // TODO: temporary
            var source = new JournalEntrySource(_parser, startTime, _logMonitor, _liveMonitor);

            var publisher = new JournalEntryPublisher(source);
            var publication = publisher.GetObservable().Publish();

            _gameContext.SubscribeTo(publication);
            _location.SubscribeTo(publication);
            _ship.SubscribeTo(publication);
            _session.SubscribeTo(publication);

            publication.Subscribe(e =>
            {
                if (e.Entry.Timestamp > (_lastEntry ?? DateTimeOffset.MinValue)) _lastEntry = e.Entry.Timestamp;
            });

            _logMonitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.File.FullName}'");
            _logMonitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.File.FullName}'");

            _location.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(l =>
                {
                    var uri = BuildLocationUri(_gameContext.CommanderName, _gameContext.GameVersion);

                    _client.Push(uri, l);
                });

            _ship.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(s =>
                {
                    var uri = BuildShipUri(_gameContext.CommanderName, _gameContext.GameVersion);
                    _client.Push(uri, s);
                });

            _session.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(s =>
                {
                    var uri = BuildSessionUri(_gameContext.CommanderName, _gameContext.GameVersion);
                    _client.Push(uri, s);
                });

            using (publication.Connect())
            {
                while (!token.IsCancellationRequested)
                {
                    Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                    publisher.Poll();

                    _client.StartUploading(token).Subscribe(t =>
                    {
                        _lastUpload = _lastUpload ?? t;
                    }, ex =>
                    {
                        if (ex is MatrixAuthenticationException)
                        {
                            OnAuthenticationRequired?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            Log.Error(ex);
                        }
                    },
                    token);

                    _lastChecked = DateTimeOffset.Now;
                    Log.Debug(_lastChecked);
                }
            }
        }

        public DateTimeOffset? LastChecked => _lastChecked;

        public DateTimeOffset? LastEntry => _lastEntry;
    }
}