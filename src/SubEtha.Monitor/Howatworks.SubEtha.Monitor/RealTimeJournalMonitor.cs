using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Howatworks.SubEtha.Parser;
using Howatworks.SubEtha.Journal;
using log4net;

namespace Howatworks.SubEtha.Monitor
{
    public class RealTimeJournalMonitor : IJournalMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RealTimeJournalMonitor));

        private readonly string _folder;
        private readonly string _filename;
        private readonly IJournalReaderFactory _journalReaderFactory;

        private bool _started;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        //private readonly IJournalReader _monitoredFile;
        private readonly CustomFileWatcher _customFileWatcher;

        private string FilePath => Path.Combine(_folder, _filename);

        public RealTimeJournalMonitor(string folder, string filename, IJournalReaderFactory journalReaderFactory)
        {
            _folder = folder;
            _filename = filename;
            _journalReaderFactory = journalReaderFactory;

            _customFileWatcher = new CustomFileWatcher(folder, filename);
            _customFileWatcher.Created += StartMonitoringFile;
            _customFileWatcher.Deleted += StopMonitoringFile;
        }

        public IList<IJournalEntry> Update(DateTimeOffset lastRead)
        {
            var entriesFound = new List<IJournalEntry>();

            if (!_started) return entriesFound;

            Log.Debug($"Rescanning file '{_filename}'...");

            entriesFound = RescanFile(lastRead).ToList();

            return entriesFound;
        }

        public IList<IJournalEntry> Start(bool firstRun, DateTimeOffset lastRead)
        {
            _customFileWatcher.Start();
            _started = true;

            // Scan any files created since last run
            return RescanFile(lastRead);
        }

        public void Stop()
        {
            _customFileWatcher.Stop();
            _started = false;
        }

        private IList<IJournalEntry> RescanFile(DateTimeOffset since)
        {
            var entries = new List<IJournalEntry>();

            if (!File.Exists(FilePath))
            {
                return entries;
            }

            Log.Debug($"Scanning file '{FilePath}'");

            var reader = _journalReaderFactory.CreateRealTimeJournalReader(FilePath);
            entries = reader.ReadAll(since).ToList();

            // Only expect one entry per standalone file, but no harm in checking
            if (entries.Count > 0)
            {
                Log.Info($"Scanned file '{FilePath}', {entries.Count} new entries found");
            }

            return entries;
        }

        private void StartMonitoringFile(string path)
        {
            JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(path));
        }

        private void StopMonitoringFile(string path)
        {
            JournalFileWatchingStopped?.Invoke(this, new JournalFileEventArgs(path));
        }

    }
}
