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
        public event EventHandler OnAuthenticationError;

        private readonly IConfiguration _config;
        private readonly IThumbLogging _logger;
        private readonly JournalMonitorScheduler _monitor;
        private readonly IThumbNotifier _notifier;
        private readonly JournalEntryRouter _router;
        private readonly HttpUploadClient _client;

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

            if (!string.IsNullOrWhiteSpace(_config["Username"]) && !string.IsNullOrWhiteSpace(_config["Password"]))
            {
                _client.AuthenticateByBearerToken(_config["Username"], _config["Password"]);
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
                    OnAuthenticationError?.Invoke(this, new EventArgs());
                }
            };
            _monitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.Path}'");

            _monitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.Path}'");
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