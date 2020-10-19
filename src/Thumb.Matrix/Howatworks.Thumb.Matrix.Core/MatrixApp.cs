using Howatworks.SubEtha.Monitor;
using Howatworks.Thumb.Core;
using System;
using Microsoft.Extensions.Configuration;
using log4net;
using Howatworks.SubEtha.Parser;
using System.Reactive.Linq;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixApp : IThumbApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixApp));

        public LocationManager Location { get; }
        public ShipManager Ship { get; }
        public SessionManager Session { get; }

        public event EventHandler OnAuthenticationRequired;

        private readonly IConfiguration _config;
        private readonly IThumbNotifier _notifier;
        private readonly HttpUploadClient _client;

        public bool IsAuthenticated => _client.IsAuthenticated;
        public string SiteUri => _client.BaseUri.AbsoluteUri;

        // Empirically-determined to match the default ASP.NET settings
        public int MaxUsernameLength = 256;
        public int MaxPasswordLength = 100;
        private readonly IJournalParser _parser;
        private readonly LogJournalMonitor _logMonitor;
        private readonly LiveJournalMonitor _liveMonitor;
        private DateTimeOffset? _lastEntry = null;
        private DateTimeOffset? _lastChecked = null;
        private readonly GameContextTracker _gameContextTracker;
        private readonly Tracker<LocationState> _locationTracker;
        private readonly Tracker<ShipState> _shipTracker;
        private readonly Tracker<SessionState> _sessionTracker;

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
            
            Location = location;
            Ship = ship;
            Session = session;
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

        public void Initialize()
        {
            Log.Info("Starting up");

            var startTime = DateTimeOffset.MinValue; // TODO: temporary
            var source = new JournalEntrySource(_parser, startTime, _logMonitor, _liveMonitor);

            var publisher = new JournalEntryPublisher(source);
            var publication = publisher.GetObservable().Publish();

            _gameContextTracker.SubscribeTo(publication);
            Location.SubscribeTo(publication);
            Ship.SubscribeTo(publication);
            Session.SubscribeTo(publication);

            publication.Subscribe(e =>
            {
                if (e.Entry.Timestamp > (_lastEntry ?? DateTimeOffset.MinValue)) _lastEntry = e.Entry.Timestamp;
            });

            _logMonitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.File.FullName}'");
            _logMonitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.File.FullName}'");

            var username = _config["Username"];
            var password = _config["Password"];

            // Try username and password from configuration, if possible
            var nowAuthenticated = Authenticate(username, password);

            // TODO: considering remove queue, shouldn't be required if we can queue up observable items
            _locationTracker.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(l =>
                {
                    var uri = BuildLocationUri(_gameContextTracker.CommanderName, _gameContextTracker.GameVersion);
                    _client.Upload(uri, l);
                });

            _shipTracker.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(s =>
                {
                    var uri = BuildShipUri(_gameContextTracker.CommanderName, _gameContextTracker.GameVersion);
                    _client.Upload(uri, s);
                });

            _sessionTracker.Observable
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(s =>
                {
                    var uri = BuildSessionUri(_gameContextTracker.CommanderName, _gameContextTracker.GameVersion);
                    _client.Upload(uri, s);
                });

            publication.Connect();
            var heartbeat = Observable.Interval(TimeSpan.FromSeconds(5))
                .Subscribe(x =>
                {
                    publisher.Poll();
                    _lastChecked = DateTimeOffset.Now;
                    Console.WriteLine(_lastChecked);
                });

                            
                


                /*try
                {
                    _locationQueue.Flush();
                }
                catch (MatrixAuthenticationException)
                {
                    OnAuthenticationRequired?.Invoke(this, EventArgs.Empty);
                }*/


        }

        private Uri BuildLocationUri(string cmdrName, string gameVersion)
        {
            return new Uri($"Api/{cmdrName}/{gameVersion}/Location", UriKind.Relative);
        }

        private Uri BuildShipUri(string cmdrName, string gameVersion)
        {
            return new Uri($"Api/{cmdrName}/{gameVersion}/Ship", UriKind.Relative);
        }

        private Uri BuildSessionUri(string cmdrName, string gameVersion)
        {
            return new Uri($"Api/{cmdrName}/{gameVersion}/Session", UriKind.Relative);
        }

        public void Shutdown()
        {
            Log.Info("Shutting down");
        }

        public bool Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            if (string.IsNullOrWhiteSpace(password)) return false;

            _client.AuthenticateByBearerToken(username, password);

            return _client.IsAuthenticated;
        }

        public void StartMonitoring()
        {
            Log.Info("Starting monitoring");
        }

        public void StopMonitoring()
        {
            Log.Info("Stopping monitoring");
        }

        public DateTimeOffset? LastEntry => _lastEntry;

        public DateTimeOffset? LastChecked => _lastChecked;
    }
}