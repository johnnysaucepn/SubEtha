using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Howatworks.PlayerJournal.Monitor;
using log4net;
using log4net.Config;

namespace Thumb.Core
{
    public class ThumbApp
    {
        private readonly IJournalMonitor _monitor;

        [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
        private readonly IJournalMonitorNotifier _notifier;

        public ThumbApp(
            IJournalMonitor monitor,
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
            //Trace.Listeners.Add(new Log4NetTraceListener(logRepository, GetType()));
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

        public DateTime? LastUpdated()
        {
            return _monitor.LastUpdated();
        }

    }
}