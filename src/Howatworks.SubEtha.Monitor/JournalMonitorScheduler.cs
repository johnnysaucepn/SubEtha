using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Howatworks.SubEtha.Parser;
using Howatworks.SubEtha.Journal;
using Microsoft.Extensions.Configuration;

namespace Howatworks.SubEtha.Monitor
{
    public class JournalMonitorScheduler : IDisposable
    {
        private readonly IList<IJournalMonitor> _journalMonitors = new List<IJournalMonitor>();
        private readonly IJournalMonitorState _journalMonitorState;
        private readonly Timer _triggerUpdate;

        public event EventHandler<JournalEntriesParsedEventArgs> JournalEntriesParsed;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        public JournalMonitorScheduler(IConfiguration config, IJournalMonitorState journalMonitorState, IJournalReaderFactory readerFactory)
        {
            _journalMonitorState = journalMonitorState;

            var folder = config["JournalFolder"];
            var pattern = config["JournalPattern"];
            var realTimeFilenames = config["RealTimeFilenames"].Split(';').Select(x => x.Trim());

            // Start with the set of all ongoing journal logs
            _journalMonitors.Add(new IncrementalJournalMonitor(folder, pattern, readerFactory));
            // Add each of the real-time, standalone files that are constantly replaced
            foreach (var filename in realTimeFilenames)
            {
                _journalMonitors.Add(new RealTimeJournalMonitor(folder, filename, readerFactory));
            }

            foreach (var monitor in _journalMonitors)
            {
                monitor.JournalFileWatchingStarted += (sender, args) => { JournalFileWatchingStarted?.Invoke(sender, args); };
                monitor.JournalFileWatchingStopped += (sender, args) => { JournalFileWatchingStopped?.Invoke(sender, args); };
            }
            var updateInterval = TimeSpan.Parse(config["UpdateInterval"]);

            _triggerUpdate = new Timer(updateInterval.TotalMilliseconds);
            _triggerUpdate.Elapsed += TriggerUpdate_Elapsed;
            _triggerUpdate.AutoReset = true;
        }

        private void TriggerUpdate_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update(e.SignalTime.ToUniversalTime(), _journalMonitorState.LastEntrySeen, BatchMode.Ongoing);
        }

        private void Update(DateTimeOffset triggerTime, DateTimeOffset? lastRead, BatchMode batchMode)
        {
            foreach (var monitor in _journalMonitors)
            {
                var entries = monitor.Update(lastRead ?? DateTimeOffset.MinValue);
                ProcessEntries(triggerTime, entries, batchMode);
            }
        }

        public void Start()
        {
            var firstRun = !_journalMonitorState.LastEntrySeen.HasValue;
            var lastRead = _journalMonitorState.LastEntrySeen ?? DateTimeOffset.MinValue;
            var triggerTime = DateTimeOffset.UtcNow;
            foreach (var monitor in _journalMonitors)
            {
                var firstEntries = monitor.Start(firstRun, lastRead);
                ProcessEntries(triggerTime, firstEntries, firstRun ? BatchMode.FirstRun : BatchMode.Catchup);
            }

            _triggerUpdate.Enabled = true;
        }

        public void Stop()
        {
            _triggerUpdate.Enabled = false;

            // One last run
            var lastRead = _journalMonitorState.LastEntrySeen;
            var triggerTime = DateTimeOffset.UtcNow;
            Update(triggerTime, lastRead, BatchMode.Ongoing);

            foreach (var monitor in _journalMonitors)
            {
                monitor.Stop();
            }

            _triggerUpdate.Dispose();
        }

        public DateTimeOffset? LastEntry()
        {
            return _journalMonitorState.LastEntrySeen;
        }

        public DateTimeOffset? LastChecked()
        {
            return _journalMonitorState.LastChecked;
        }

        private void ProcessEntries(DateTimeOffset triggerTime, IList<IJournalEntry> journalEntries, BatchMode mode)
        {
            if (journalEntries.Count == 0) return;

            JournalEntriesParsed?.Invoke(this, new JournalEntriesParsedEventArgs(journalEntries, mode));
            _journalMonitorState.Update(triggerTime, journalEntries.Max(x => x.Timestamp));
        }

        public void Dispose()
        {
            _triggerUpdate?.Dispose();
        }
    }
}
