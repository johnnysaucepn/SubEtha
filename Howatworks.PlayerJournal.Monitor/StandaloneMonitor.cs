using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.PlayerJournal.Monitor
{
    public class StandaloneMonitor : IJournalMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StandaloneMonitor));

        private readonly IJournalReaderFactory _journalReaderFactory;
        private readonly FileSystemWatcher _journalWatcher;

        private bool _started = false;

        private readonly string _filename;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        private IJournalReader _monitoredFile;

        public StandaloneMonitor(IConfiguration config, IJournalReaderFactory journalReaderFactory)
        {
            _journalReaderFactory = journalReaderFactory;

            // TODO: this makes the config read-only; consider keeping hold of the config object and reacting to config changes
            var journalFolder = config["JournalFolder"];
            _filename = config["StatusFilename"];

            var filePath = Path.Combine(journalFolder, _filename);

            _journalWatcher = new FileSystemWatcher(journalFolder, _filename)
            {
                EnableRaisingEvents = false
            };

            _journalWatcher.Created += (s, e) =>
            {
                LogWatcherEvent(e);
                StartMonitoringFile(e.FullPath);
            };

            _journalWatcher.Deleted += (s, e) =>
            {
                LogWatcherEvent(e);
                StopMonitoringFile(e.FullPath);
            };

            if (File.Exists(filePath))
            {
                StartMonitoringFile(filePath);
            }

        }

        public IList<IJournalEntry> Update(DateTime lastRead)
        {
            var entriesFound = new List<IJournalEntry>();

            if (!_started) return entriesFound;

            lock (_monitoredFile)
            {
                if (_monitoredFile == null) return entriesFound;

                Log.Debug($"Rescanning {_filename} file...");

                // TODO: this list assignment should be unnecessary
                entriesFound = RescanFile(_monitoredFile, lastRead).ToList();
            }

            return entriesFound;
        }

        public IList<IJournalEntry> Start(bool firstRun, DateTime lastRead)
        {
            // Scan any files created since last run
            if (_monitoredFile == null) return new List<IJournalEntry>();

            // TODO: this list assignment should be unnecessary
            IList<IJournalEntry> firstEntries = RescanFile(_monitoredFile, lastRead).ToList();

            _journalWatcher.EnableRaisingEvents = true;
            _started = true;
            return firstEntries;
        }

        public void Stop()
        {
            _started = false;

            _journalWatcher.EnableRaisingEvents = false;

        }

        private IEnumerable<IJournalEntry> RescanFile(IJournalReader reader, DateTimeOffset since)
        {
            Log.Debug($"Scanning file {reader.FilePath}");
            var count = 0;

            if (reader.FileExists)
            {
                foreach (var entry in reader.ReadAll(since))
                {
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

        private static void LogWatcherEvent(FileSystemEventArgs e)
        {
            Log.Info($"Received {e.ChangeType} entry on file {e.FullPath}");
        }

        private void StartMonitoringFile(string path)
        {
            _monitoredFile = _journalReaderFactory.Create(path);
            JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(_monitoredFile.FilePath));
            //TODO: should check validity of IJournalReader before adding it
        }


        private void StopMonitoringFile(string path)
        {
            JournalFileWatchingStopped?.Invoke(this, new JournalFileEventArgs(path));

            if (_monitoredFile == null)
            {
                Log.Error($"Not monitoring file {path} - cannot stop");
            }
            _monitoredFile = null;
        }

    }
}
