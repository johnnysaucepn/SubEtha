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
        private readonly HttpUploadClient _client;

        private readonly IConfiguration _config;
        private DateTimeOffset? _lastChecked = null;
        private DateTimeOffset? _lastEntry = null;
        private DateTimeOffset? _lastUpload = null;
        private readonly LiveJournalMonitor _liveMonitor;

        private readonly GameContextTracker _gameContextTracker;
        private readonly LocationManager _location;
        private readonly Tracker<LocationState> _locationTracker;
        private readonly LogJournalMonitor _logMonitor;
        private readonly IThumbNotifier _notifier;
        private readonly IJournalParser _parser;
        private readonly SessionManager _session;
        private readonly Tracker<SessionState> _sessionTracker;
        private readonly ShipManager _ship;
        private readonly Tracker<ShipState> _shipTracker;
        public int MaxPasswordLength = 100;

        // Empirically-determined to match the default ASP.NET settings
        public int MaxUsernameLength = 256;

        public MatrixApp(
            IConfiguration config,
            LogJournalMonitor logMonitor,
            LiveJournalMonitor liveMonitor,
            IThumbNotifier notifier,
            IJournalParser parser,
            GameContextTracker gameContextTracker,

            LocationManager location,
            Tracker<LocationState> locationTracker,

            ShipManager ship,
            Tracker<ShipState> shipTracker,

            SessionManager session,
            Tracker<SessionState> sessionTracker,

            HttpUploadClient client
        )
        {
            _location = location;
            _ship = ship;
            _session = session;
            _config = config;
            _logMonitor = logMonitor;
            _liveMonitor = liveMonitor;

            _gameContextTracker = gameContextTracker;
            _locationTracker = locationTracker;
            _shipTracker = shipTracker;
            _sessionTracker = sessionTracker;

            _notifier = notifier;
            _parser = parser;

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

            _gameContextTracker.SubscribeTo(publication);
            _location.SubscribeTo(publication);
            _ship.SubscribeTo(publication);
            _session.SubscribeTo(publication);

            publication.Subscribe(e =>
            {
                if (e.Entry.Timestamp > (_lastEntry ?? DateTimeOffset.MinValue)) _lastEntry = e.Entry.Timestamp;
            });

            _logMonitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.File.FullName}'");
            _logMonitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.File.FullName}'");

            _locationTracker.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(l =>
                {
                    var uri = BuildLocationUri(_gameContextTracker.CommanderName, _gameContextTracker.GameVersion);

                    _client.Push(uri, l);
                });

            _shipTracker.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(s =>
                {
                    var uri = BuildShipUri(_gameContextTracker.CommanderName, _gameContextTracker.GameVersion);
                    _client.Push(uri, s);
                });

            _sessionTracker.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(s =>
                {
                    var uri = BuildSessionUri(_gameContextTracker.CommanderName, _gameContextTracker.GameVersion);
                    _client.Push(uri, s);
                });

            publication.Connect();

            while (!token.IsCancellationRequested)
            {
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                publisher.Poll();

                _client.StartUploading(token).Subscribe(t =>
                {
                    _lastUpload = _lastUpload ?? t;
                }, ex =>
                {
                    OnAuthenticationRequired?.Invoke(this, EventArgs.Empty);
                },

                () =>
                {
                    Log.Info("Upload complete");
                });

                _lastChecked = DateTimeOffset.Now;
                Console.WriteLine(_lastChecked);
            }
        }

        public void Shutdown()
        {
            Log.Info("Shutting down");
        }

        public void StartMonitoring()
        {
            Log.Info("Starting monitoring");
        }

        public void StopMonitoring()
        {
            Log.Info("Stopping monitoring");
        }

        public DateTimeOffset? LastChecked => _lastChecked;

        public DateTimeOffset? LastEntry => _lastEntry;
    }
}