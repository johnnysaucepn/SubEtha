using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Other;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.PlayerJournal.Monitor
{
    public class JournalMonitor :  IJournalMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalMonitor));


        private readonly IJournalReaderFactory _journalReaderFactory;
        private readonly FileSystemWatcher _journalWatcher;

        private bool _started = false;

        private readonly string _journalFolder;
        private readonly string _journalPattern;

        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        private readonly Dictionary<string, IJournalReader> _monitoredFiles = new Dictionary<string, IJournalReader>();

        public JournalMonitor(IConfiguration config, IJournalReaderFactory journalReaderFactory)
        {
            _journalReaderFactory = journalReaderFactory;

            // TODO: this makes the config read-only; consider keeping hold of the config object and reacting to config changes
            _journalFolder = config["JournalFolder"];
            _journalPattern = config["JournalPattern"];

            _journalWatcher = new FileSystemWatcher(_journalFolder, _journalPattern)
            {
                EnableRaisingEvents = false
            };

            _journalWatcher.Created += (s, e) =>
            {
                LogWatcherEvent(e);
                StartMonitoringFile(e.FullPath);
            };
            _journalWatcher.Changed += (s, e) =>
            {
                LogWatcherEvent(e);
                StartMonitoringFile(e.FullPath);
            };

            _journalWatcher.Deleted += (s, e) =>
            {
                LogWatcherEvent(e);
                StopMonitoringFile(e.FullPath);
            };

        }


        public IList<IJournalEntry> Update(DateTime lastRead)
        {
            var entriesFound = new List<IJournalEntry>();

            if (!_started) return entriesFound;

            lock (_monitoredFiles)
            {
                if (_monitoredFiles.Count <= 0) return entriesFound;

                Log.Debug($"Rescanning {_monitoredFiles.Count} log files...");

                // TODO: this appears to be required to avoid collection being modified during enumeration
                // Unclear as to how this can happen
                var readers = _monitoredFiles.Values.ToList();
                entriesFound = RescanFiles(readers, lastRead).ToList();
            }

            return entriesFound;
        }

        public IList<IJournalEntry> Start(bool firstRun, DateTime lastRead)
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

            _journalWatcher.EnableRaisingEvents = true;
            _started = true;
            return firstEntries;
        }

        public void Stop()
        {
            _started = false;

            _journalWatcher.EnableRaisingEvents = false;

        }



        private static IEnumerable<string> EnumerateFolder(string path, string pattern)
        {
            return Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly);
        }

        private IEnumerable<IJournalReader> FilterFilesByDate(IEnumerable<string> filePaths, DateTimeOffset since)
        {
            return filePaths.Select(filePath =>
                {
                    var reader = _journalReaderFactory.Create(filePath);
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

        private static void LogWatcherEvent(FileSystemEventArgs e)
        {
            Log.Info($"Received {e.ChangeType} entry on file {e.FullPath}");
        }

        private void StartMonitoringFile(string path)
        {
            StartMonitoringFile(_journalReaderFactory.Create(path));
            //TODO: should check validity of IJournalReader before adding it
        }

        private void StartMonitoringFile(IJournalReader reader)
        {
            lock (_monitoredFiles)
            {
                if (_monitoredFiles.ContainsKey(reader.FilePath)) return;
                _monitoredFiles.Add(reader.FilePath, reader);
            }

            JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(reader.FilePath));
        }

        private void StopMonitoringFile(string path)
        {
            JournalFileWatchingStopped?.Invoke(this, new JournalFileEventArgs(path));

            lock (_monitoredFiles)
            {
                if (_monitoredFiles.ContainsKey(path))
                {
                    _monitoredFiles.Remove(path);
                }
                else
                {
                    Log.Error($"Not monitoring file {path} - cannot stop");
                }
            }
        }

    }
}
