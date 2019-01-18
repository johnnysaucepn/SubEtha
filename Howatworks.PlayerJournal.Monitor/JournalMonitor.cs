using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Other;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.PlayerJournal.Monitor
{
    public class JournalMonitor : IDisposable, IJournalMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalMonitor));

        private readonly IJournalMonitorState _journalMonitorState;
        private readonly IJournalReaderFactory _journalReaderFactory;
        private readonly FileSystemWatcher _journalWatcher;

        private bool _started = false;

        private readonly string _journalFolder;
        private readonly string _journalPattern;
        private readonly string _statusPath;

        public event EventHandler<JournalEntriesParsedEventArgs> JournalEntriesParsed;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        private readonly Timer _triggerUpdate;
        private readonly Dictionary<string, IJournalReader> _monitoredFiles = new Dictionary<string, IJournalReader>();

        public JournalMonitor(IConfiguration config, IJournalMonitorState journalMonitorState, IJournalReaderFactory journalReaderFactory)
        {
            _journalMonitorState = journalMonitorState;
            _journalReaderFactory = journalReaderFactory;

            // TODO: this makes the config read-only; consider keeping hold of the config object and reacting to config changes
            _journalFolder = config["JournalFolder"];
            _journalPattern = config["JournalPattern"];
            _statusPath = config["StatusPath"];
            var updateInterval = TimeSpan.Parse(config["UpdateInterval"]);

            _journalWatcher = new FileSystemWatcher(_journalFolder, _journalPattern)
            {
                EnableRaisingEvents = false
            };

            _journalWatcher.Created += (s, e) => { LogWatcherEvent(e); StartMonitoringFile(e.FullPath); };
            _journalWatcher.Changed += (s, e) => { LogWatcherEvent(e); StartMonitoringFile(e.FullPath); };

            _triggerUpdate = new Timer(updateInterval.TotalMilliseconds);
            _triggerUpdate.Elapsed += (o, args) => TriggerUpdate_Elapsed(_journalMonitorState.LastRead);
            _triggerUpdate.AutoReset = true;
        }


        private void TriggerUpdate_Elapsed(DateTime? lastRead)
        {
            if (!_started) return;

            lock (_monitoredFiles)
            {
                if (_monitoredFiles.Count <= 0) return;

                Log.Debug($"Rescanning {_monitoredFiles.Count} log files...");

                // TODO: this appears to be required to avoid collection being modified during enumeration
                // Unclear as to how this can happen
                var readers = _monitoredFiles.Values.ToList();
                var entriesFound = RescanFiles(readers, lastRead.GetValueOrDefault(DateTime.MinValue)).ToList();
                ProcessEntries(entriesFound, BatchMode.Ongoing);
            }
        }

        public void Start()
        {
            var firstRun = !_journalMonitorState.LastRead.HasValue;
            var lastRead = _journalMonitorState.LastRead.GetValueOrDefault(DateTime.MinValue);

            // Scan any files created since last run

            var allFilesInFolder = EnumerateFolder(_journalFolder, _journalPattern);
            var recentFilesInFolder = FilterFilesByDate(allFilesInFolder, lastRead).ToList();

            if (recentFilesInFolder.Any())
            {
                var entriesFound = RescanFiles(recentFilesInFolder, lastRead).ToList();
                ProcessEntries(entriesFound, firstRun ? BatchMode.FirstRun : BatchMode.Catchup);

                // If we've found some files changed since last run, the last one *might* be active.
                StartMonitoringFile(recentFilesInFolder.Last());
            }

            StartMonitoringFile(Path.Combine(_journalFolder, _statusPath));

            _journalWatcher.EnableRaisingEvents = true;
            _triggerUpdate.Enabled = true;
            _started = true;
        }

        public void Stop()
        {
            _started = false;
            _triggerUpdate.Enabled = false;
            _journalWatcher.EnableRaisingEvents = false;

            var lastRead = _journalMonitorState.LastRead;

            // One last scan of live files
            List<IJournalEntry> entriesFound;
            lock (_monitoredFiles)
            {
                entriesFound = RescanFiles(_monitoredFiles.Values, lastRead).ToList();
            }
            ProcessEntries(entriesFound, BatchMode.Ongoing);
            _triggerUpdate.Dispose();
        }

        private void ProcessEntries(IList<IJournalEntry> journalEntries, BatchMode mode)
        {
            if (!journalEntries.Any()) return;

            JournalEntriesParsed?.Invoke(this, new JournalEntriesParsedEventArgs(journalEntries, mode));
            _journalMonitorState.LastRead = journalEntries.OrderBy(x => x.Timestamp).Last().Timestamp;
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

        private IEnumerable<IJournalEntry> RescanFiles(IEnumerable<IJournalReader> readers, DateTimeOffset? since)
        {
            return readers.SelectMany(reader => RescanFile(reader, since));
        }

        private IEnumerable<IJournalEntry> RescanFile(IJournalReader reader, DateTimeOffset? since)
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


        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _triggerUpdate.Dispose();
            }

            _disposed = true;
        }

        public DateTime? LastUpdated()
        {
            return _journalMonitorState.LastRead;
        }
    }
}
