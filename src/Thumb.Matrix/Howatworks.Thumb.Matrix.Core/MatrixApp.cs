using Howatworks.SubEtha.Monitor;
using Howatworks.Thumb.Core;
using System;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixApp : IThumbApp
    {
        public LocationManager Location { get; }
        public ShipManager Ship { get; set; }
        public SessionManager Session { get; set; }

        public event EventHandler OnAuthenticationRequired;

        private readonly IConfiguration _config;
        private readonly IThumbLogging _logger;
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
            IThumbLogging logger,
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
            _logger = logger;
            _monitor = monitor;
            _notifier = notifier;
            _router = router;
            _client = client;
        }

        public void Initialize()
        {
            _logger.Configure();

            // Try username and password from configuration, if possible
            if (!IsAuthenticated)
            {
                if (!string.IsNullOrWhiteSpace(_config["Username"]) && !string.IsNullOrWhiteSpace(_config["Password"]))
                {
                    Authenticate(_config["Username"], _config["Password"]);
                }
            }
            // Otherwise, delegate getting username and password to caller
            // TODO: make this more structured
            if (!IsAuthenticated)
            {
                OnAuthenticationRequired?.Invoke(this, EventArgs.Empty);
            }

            _monitor.JournalEntriesParsed += (sender, args) =>
            {
                if (args == null) return;
                try
                {
                    _router.Apply(args.Entries, args.BatchMode);
                }
                catch (MatrixAuthenticationException)
                {
                    OnAuthenticationRequired?.Invoke(this, EventArgs.Empty);
                }
            };
            _monitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.Path}'");

            _monitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.Path}'");
        }

        public bool Authenticate(string username, string password)
        {
            _client.AuthenticateByBearerToken(username, password);
            return _client.IsAuthenticated;
        }

        public void Start()
        {
            _monitor.Start();
        }

        public void Stop()
        {
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