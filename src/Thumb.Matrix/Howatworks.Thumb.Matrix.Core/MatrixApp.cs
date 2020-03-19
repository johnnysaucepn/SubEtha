using Howatworks.SubEtha.Monitor;
using Howatworks.Thumb.Core;
using System;
using Microsoft.Extensions.Configuration;
using log4net;

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
        private readonly JournalMonitorScheduler _monitor;
        private readonly IThumbNotifier _notifier;
        private readonly JournalEntryRouter _router;
        private readonly HttpUploadClient _client;

        public bool IsAuthenticated => _client.IsAuthenticated;
        public string SiteUri => _client.BaseUri.AbsoluteUri;

        // Empirically-determined to match the default ASP.NET settings
        public int MaxUsernameLength = 256;
        public int MaxPasswordLength = 100;

        public MatrixApp(
            IConfiguration config,
            JournalMonitorScheduler monitor,
            IThumbNotifier notifier,
            JournalEntryRouter router,
            LocationManager location,
            ShipManager ship,
            SessionManager session,
            HttpUploadClient client
        )
        {
            Location = location;
            Ship = ship;
            Session = session;
            _config = config;
            _monitor = monitor;
            _notifier = notifier;
            _router = router;
            _client = client;
        }

        public void Initialize()
        {
            Log.Info("Starting up");

            _monitor.JournalEntriesParsed += (sender, args) =>
            {
                if (args == null) return;

                _router.Apply(args.Entries, args.BatchMode);

                try
                {
                    Location.FlushQueue();
                    Ship.FlushQueue();
                    Session.FlushQueue();
                }
                catch (MatrixAuthenticationException)
                {
                    OnAuthenticationRequired?.Invoke(this, EventArgs.Empty);
                }
            };
            _monitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.Path}'");

            _monitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.Path}'");

            var username = _config["Username"];
            var password = _config["Password"];

            // Try username and password from configuration, if possible
            var nowAuthenticated = Authenticate(username, password);
        }

        public void Shutdown()
        {
            Log.Info("Shutting down");
            StopMonitoring();
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
            _monitor.Start();
        }

        public void StopMonitoring()
        {
            Log.Info("Stopping monitoring");
            _monitor.Stop();
        }

        public DateTimeOffset? LastEntry()
        {
            return _monitor.LastEntry();
        }

        public DateTimeOffset? LastChecked()
        {
            return _monitor.LastChecked();
        }
    }
}