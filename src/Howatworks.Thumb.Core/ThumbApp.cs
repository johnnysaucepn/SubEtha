using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Monitor;
using log4net;
using log4net.Config;

namespace Howatworks.Thumb.Core
{
    public class ThumbApp
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ThumbApp));

        private readonly JournalMonitorScheduler _monitor;

        [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
        private readonly IThumbNotifier _notifier;

        private readonly JournalEntryRouter _router;

        public ThumbApp(
            JournalMonitorScheduler monitor,
            IThumbNotifier notifier,
            JournalEntryRouter router
        )
        {
            _monitor = monitor;
            _notifier = notifier;
            _router = router;
            _monitor.JournalEntriesParsed += (sender, args) =>
            {
                if (args == null) return;
                Apply(args.Entries, args.BatchMode);
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
        }

        public void Start()
        {
            _monitor.Start();
        }

        public void Stop()
        {
            _monitor.Stop();
        }

        public void Apply(IEnumerable<IJournalEntry> entries, BatchMode mode)
        {
            foreach (var journalEntry in entries)
            {
                var somethingApplied = _router.Apply(journalEntry, mode);
                if (!somethingApplied)
                {
                    Log.Info($"No handler applied for event type {journalEntry.Event}");
                }
            }

            var batchProcessed = _router.ApplyBatchComplete(mode);
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