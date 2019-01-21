using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Microsoft.Extensions.Configuration;

namespace Howatworks.PlayerJournal.Monitor
{
    public class JournalMonitorScheduler : IDisposable
    {
        private readonly IEnumerable<IJournalMonitor> _journalMonitors;
        private readonly IJournalMonitorState _journalMonitorState;
        private readonly Timer _triggerUpdate;

        public event EventHandler<JournalEntriesParsedEventArgs> JournalEntriesParsed;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        public JournalMonitorScheduler(IConfiguration config, IJournalMonitorState journalMonitorState, IJournalReaderFactory readerFactory)
        {
            _journalMonitorState = journalMonitorState;
            _journalMonitors = new List<IJournalMonitor>
            {
                new JournalMonitor(config, readerFactory),
                new StandaloneMonitor(config, readerFactory)
            };

            foreach (var monitor in _journalMonitors)
            {
                monitor.JournalFileWatchingStarted += (sender, args) => { JournalFileWatchingStarted?.Invoke(sender, args); };
                monitor.JournalFileWatchingStopped += (sender, args) => { JournalFileWatchingStopped?.Invoke(sender, args); };
            }
            var updateInterval = TimeSpan.Parse(config["UpdateInterval"]);

            _triggerUpdate = new Timer(updateInterval.TotalMilliseconds);
            _triggerUpdate.Elapsed += _triggerUpdate_Elapsed;
            _triggerUpdate.AutoReset = true;
        }

        private void _triggerUpdate_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update(_journalMonitorState.LastRead.GetValueOrDefault(DateTime.MinValue), BatchMode.Ongoing);
        }

        public void Update(DateTime lastRead, BatchMode batchMode)
        {
            foreach (var monitor in _journalMonitors)
            {
                var entries = monitor.Update(lastRead);
                ProcessEntries(entries, batchMode);
            }
        }

        public void Start()
        {
            var firstRun = !_journalMonitorState.LastRead.HasValue;
            var lastRead = _journalMonitorState.LastRead.GetValueOrDefault(DateTime.MinValue);
            foreach (var monitor in _journalMonitors)
            {
                var firstEntries = monitor.Start(firstRun, lastRead);
                ProcessEntries(firstEntries, firstRun ? BatchMode.FirstRun : BatchMode.Catchup);
            }

            _triggerUpdate.Enabled = true;
        }

        public void Stop()
        {
            _triggerUpdate.Enabled = false;

            // One last run
            var lastRead = _journalMonitorState.LastRead.GetValueOrDefault(DateTime.MinValue);
            Update(lastRead, BatchMode.Ongoing);

            _triggerUpdate.Dispose();
        }

        public DateTime? LastUpdated()
        {
            return _journalMonitorState.LastRead;
        }


        private void ProcessEntries(IList<IJournalEntry> journalEntries, BatchMode mode)
        {
            if (!journalEntries.Any()) return;

            JournalEntriesParsed?.Invoke(this, new JournalEntriesParsedEventArgs(journalEntries, mode));
            _journalMonitorState.LastRead = journalEntries.OrderBy(x => x.Timestamp).Last().Timestamp;
        }

        public void Dispose()
        {
            _triggerUpdate?.Dispose();
        }
    }
}
