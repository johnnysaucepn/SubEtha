using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Other;
using log4net;

namespace Howatworks.PlayerJournal.Monitor
{
    public class IncrementalJournalMonitor :  IJournalMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IncrementalJournalMonitor));

        private readonly IJournalReaderFactory _journalReaderFactory;
        private readonly CustomFileWatcher _journalWatcher;

        private bool _started;

        private readonly string _journalFolder;
        private readonly string _journalPattern;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        private readonly Dictionary<string, IJournalReader> _activeReaders = new Dictionary<string, IJournalReader>();

        public IncrementalJournalMonitor(string folder, string pattern, IJournalReaderFactory journalReaderFactory)
        {
            _journalReaderFactory = journalReaderFactory;

            // TODO: this makes the config read-only; consider keeping hold of the config object and reacting to config changes
            _journalFolder = folder;
            _journalPattern = pattern;

            _journalWatcher = new CustomFileWatcher(_journalFolder, _journalPattern);
            _journalWatcher.Created += StartMonitoringFile;
            _journalWatcher.Changed += StartMonitoringFile;
            _journalWatcher.Deleted += StopMonitoringFile;
        }

        public IList<IJournalEntry> Update(DateTimeOffset lastRead)
        {
            var entriesFound = new List<IJournalEntry>();

            if (!_started) return entriesFound;

            lock (_activeReaders)
            {
                if (_activeReaders.Count <= 0) return entriesFound;

                Log.Debug($"Rescanning {_activeReaders.Count} log files...");

                // TODO: this is required to avoid collection being modified during enumeration
                // Should note the readers to be removed after the operation
                var readers = _activeReaders.Values.ToList();
                entriesFound = RescanFiles(readers, lastRead).ToList();
            }

            return entriesFound;
        }

        public IList<IJournalEntry> Start(bool firstRun, DateTimeOffset lastRead)
        {
            IList<IJournalEntry> firstEntries = new List<IJournalEntry>();

            // Scan any files created since last run

            var allFilesInFolder = EnumerateFolder(_journalFolder, _journalPattern);
            var recentFilesInFolder = FilterFilesByDate(allFilesInFolder, lastRead).ToList();

            if (recentFilesInFolder.Any())
            {
                firstEntries = RescanFiles(recentFilesInFolder, lastRead).ToList();

                // If we've found some files changed since last run, the last one *might* be active.
                StartMonitoringFile(recentFilesInFolder.Last());
            }

            _journalWatcher.Start();
            _started = true;
            return firstEntries;
        }

        public void Stop()
        {
            _started = false;

            _journalWatcher.Stop();
        }

        private static IEnumerable<string> EnumerateFolder(string path, string pattern)
        {
            return Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly);
        }

        private IEnumerable<IJournalReader> FilterFilesByDate(IEnumerable<string> filePaths, DateTimeOffset since)
        {
            return filePaths.Select(filePath =>
                {
                    var reader = _journalReaderFactory.CreateIncrementalJournalReader(filePath);
                    //var info = reader.FileInfo;

                    // If no header entry found, log is not a log file (yet).
                    // If it later gets a header, eventually we will be notified to start
                    // monitoring it, and we'll read it then.
                    //if (!info.IsValid) return null;

                    return reader.LastEntryTimeStamp.GetValueOrDefault(DateTimeOffset.MaxValue) > since ? reader : null;
                })
                .Where(f => f != null)
                .OrderBy(f => f.LastEntryTimeStamp);
        }

        private IEnumerable<IJournalEntry> RescanFiles(IEnumerable<IJournalReader> readers, DateTimeOffset since)
        {
            return readers.SelectMany(reader => RescanFile(reader, since));
        }

        private IEnumerable<IJournalEntry> RescanFile(IJournalReader reader, DateTimeOffset since)
        {
            //var info = reader.FileInfo;

            Log.Debug($"Scanning file {reader.FilePath}");
            var count = 0;

            if (reader.FileExists)
            {
                foreach (var entry in reader.ReadAll(since))
                {
                    // Special case for 'Continued' journal entry - stop monitoring this file after this, as
                    // logging should be rolling over to a new file
                    if (entry is Continued)
                    {
                        StopMonitoringFile(reader.FilePath);
                    }

                    count++;
                    yield return entry;
                }
            }
            else
            {
                StopMonitoringFile(reader.FilePath);
            }

            if (count > 0)
            {
                Log.Info($"Scanned file {reader.FilePath}, {count} new entries found");
            }
        }

        private void StartMonitoringFile(string path)
        {
            var reader = _journalReaderFactory.CreateIncrementalJournalReader(path);
            StartMonitoringFile(reader);
        }

        private void StartMonitoringFile(IJournalReader reader)
        {
            lock (_activeReaders)
            {
                if (_activeReaders.ContainsKey(reader.FilePath)) return;
                _activeReaders.Add(reader.FilePath, reader);
                //TODO: should check validity of IJournalReader before adding it
            }

            JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(reader.FilePath));
        }

        private void StopMonitoringFile(string path)
        {
            JournalFileWatchingStopped?.Invoke(this, new JournalFileEventArgs(path));

            lock (_activeReaders)
            {
                if (_activeReaders.ContainsKey(path))
                {
                    _activeReaders.Remove(path);
                }
                else
                {
                    Log.Error($"Not monitoring file {path} - cannot stop");
                }
            }
        }

    }
}
