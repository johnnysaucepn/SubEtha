using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Howatworks.SubEtha.Monitor;
using log4net;
using log4net.Config;

namespace Howatworks.Thumb.Core
{
    public class ThumbApp
    {
        private readonly JournalMonitorScheduler _monitor;

        [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
        private readonly IThumbNotifier _notifier;

        public ThumbApp(
            JournalMonitorScheduler monitor,
            IThumbNotifier notifier,
            ThumbProcessor processor
        )
        {
            _monitor = monitor;
            _notifier = notifier;
            _monitor.JournalEntriesParsed += (sender, args) =>
            {
                if (args == null) return;
                processor.Apply(args.Entries, args.BatchMode);
            };
            _monitor.JournalFileWatchingStarted += (sender, args) => _notifier.Notify(NotificationPriority.High, NotificationEventType.FileSystem, $"Started watching '{args.Path}'");

            _monitor.JournalFileWatchingStopped += (sender, args) => _notifier.Notify(NotificationPriority.Medium, NotificationEventType.FileSystem, $"Stopped watching '{args.Path}'");

            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var workingFolder = Path.Combine(appDataFolder, "Howatworks", "Thumb");
            var logFolder = Path.Combine(workingFolder, "Logs");
            Directory.CreateDirectory(logFolder);

            GlobalContext.Properties["logfolder"] = logFolder;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            processor.Startup();
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