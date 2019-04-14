using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Howatworks.SubEtha.Monitor;
using log4net;
using log4net.Config;
using Howatworks.Thumb.Plugin;

namespace Howatworks.Thumb.Core
{
    public class ThumbApp
    {
        private readonly JournalMonitorScheduler _monitor;

        [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
        private readonly IJournalMonitorNotifier _notifier;

        public ThumbApp(
            JournalMonitorScheduler monitor,
            IJournalMonitorNotifier notifier,
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
            _monitor.JournalFileWatchingStarted += (sender, args) => { _notifier.StartedWatchingFile(args.Path); };

            _monitor.JournalFileWatchingStopped += (sender, args) => { _notifier.StoppedWatchingFile(args.Path); };

            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var workingFolder = Path.Combine(appDataFolder, "Howatworks", "Thumb");
            var logFolder = Path.Combine(workingFolder, "Logs");
            Directory.CreateDirectory(logFolder);

            GlobalContext.Properties["logfolder"] = logFolder;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
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