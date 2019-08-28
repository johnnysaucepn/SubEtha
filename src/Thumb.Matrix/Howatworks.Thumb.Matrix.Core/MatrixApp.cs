using Howatworks.SubEtha.Monitor;
using Howatworks.Thumb.Core;
using System;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixApp : IThumbApp
    {
        public LocationManager Location { get; }
        public ShipManager Ship { get; set; }
        public SessionManager Session { get; set; }

        private readonly IThumbLogging _logger;
        private readonly JournalMonitorScheduler _monitor;
        private readonly IThumbNotifier _notifier;
        private readonly JournalEntryRouter _router;

        public MatrixApp(
            IThumbLogging logger,
            JournalMonitorScheduler monitor,
            IThumbNotifier notifier,
            JournalEntryRouter router,
            LocationManager location,
            ShipManager ship,
            SessionManager session
        )
        {
            Location = location;
            Ship = ship;
            Session = session;
            _logger = logger;
            _monitor = monitor;
            _notifier = notifier;
            _router = router;
        }

        public void Initialize()
        {
            _logger.Configure();

            _monitor.JournalEntriesParsed += (sender, args) =>
            {
                if (args == null) return;
                _router.Apply(args.Entries, args.BatchMode);
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