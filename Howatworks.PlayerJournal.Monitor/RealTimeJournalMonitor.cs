using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using log4net;

namespace Howatworks.PlayerJournal.Monitor
{
    public class RealTimeJournalMonitor : IJournalMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RealTimeJournalMonitor));

        private readonly string _folder;
        private readonly string _filename;

        private bool _started = false;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        private readonly IJournalReader _monitoredFile;
        private readonly CustomFileWatcher _customFileWatcher;

        private string FilePath => Path.Combine(_folder, _filename);

        public RealTimeJournalMonitor(string folder, string filename, IJournalReaderFactory journalReaderFactory)
        {
            _folder = folder;
            _filename = filename;

            _customFileWatcher = new CustomFileWatcher(folder, filename, StartMonitoringFile, StopMonitoringFile);
            _monitoredFile = journalReaderFactory.CreateRealTimeJournalReader(FilePath);
        }

        public IList<IJournalEntry> Update(DateTime lastRead)
        {
            var entriesFound = new List<IJournalEntry>();

            if (!_started) return entriesFound;

            lock (_monitoredFile)
            {
                Log.Debug($"Rescanning {_filename} file...");

                entriesFound = RescanFile(lastRead).ToList();
            }

            return entriesFound;
        }

        public IList<IJournalEntry> Start(bool firstRun, DateTime lastRead)
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

            if (!_monitoredFile.FileExists)
            {
                return entries;
            }

            Log.Debug($"Scanning file {_monitoredFile.FilePath}");
            entries = _monitoredFile.ReadAll(since).ToList();

            // Only expect one entry per standalone file, but no harm in checking
            if (entries.Count > 0)
            {
                Log.Info($"Scanned file {_monitoredFile.FilePath}, {entries.Count} new entries found");
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
