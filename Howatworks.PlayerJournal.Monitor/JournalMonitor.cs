using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Other;
using log4net;

namespace Howatworks.PlayerJournal.Monitor
{
    public class JournalMonitor : IDisposable, IJournalMonitor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalMonitor));

        private readonly IJournalMonitorConfiguration _config;
        private readonly IJournalReaderFactory _journalReaderFactory;
        private readonly FileSystemWatcher _journalWatcher;

        private bool _started = false;

        public event EventHandler<JournalEntriesParsedEventArgs> JournalEntriesParsed;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        private DateTime? LastRead
        {
            get => _config.LastRead;
            set => _config.LastRead = value;
        }

        private readonly Timer _triggerUpdate;
        private readonly Dictionary<string, IJournalReader> _monitoredFiles = new Dictionary<string, IJournalReader>();

        public JournalMonitor(IJournalMonitorConfiguration config, IJournalReaderFactory journalReaderFactory)
        {
            _config = config;
            _journalReaderFactory = journalReaderFactory;

            _journalWatcher = new FileSystemWatcher(_config.JournalFolder, _config.JournalPattern)
            {
                EnableRaisingEvents = false
            };
            _journalWatcher.Created += (s, e) => { LogWatcherEvent(e); StartMonitoringFile(e.FullPath); };
            _journalWatcher.Changed += (s, e) => { LogWatcherEvent(e); StartMonitoringFile(e.FullPath); };

            var updatePeriod = Convert.ToInt32(_config.UpdateInterval.TotalMilliseconds);
            _triggerUpdate = new Timer(TriggerUpdate_Elapsed, null, updatePeriod, updatePeriod);
        }

        private void TriggerUpdate_Elapsed(object data)
        {
            if (!_started) return;

            lock (_monitoredFiles)
            {
                if (_monitoredFiles.Count <= 0) return;

                Log.Debug($"Rescanning {_monitoredFiles.Count} log files...");

                // TODO: this appears to be required to avoid collection being modified during enumeration
                // Unclear as to how this can happen
                var readers = _monitoredFiles.Values.ToList();
                var entriesFound = RescanFiles(readers, LastRead).ToList();
                ProcessEntries(entriesFound, BatchMode.Ongoing);
            }
        }

        public void Start()
        {
            // Scan any files created since last run

            var allFilesInFolder = EnumerateFolder(_config.JournalFolder, _config.JournalPattern);
            var recentFilesInFolder = FilterFilesByDate(allFilesInFolder, LastRead).ToList();
            
            if (recentFilesInFolder.Any())
            {
                var entriesFound = RescanFiles(recentFilesInFolder, LastRead).ToList();
                ProcessEntries(entriesFound, LastRead.HasValue ? BatchMode.Catchup : BatchMode.FirstRun);

                // If we've found some files changed since last run, the last one *might* be active.
                StartMonitoringFile(recentFilesInFolder.Last());
            }

            StartMonitoringFile(Path.Combine(_config.StatusPath));

            _journalWatcher.EnableRaisingEvents = true;
            _started = true;
        }

        public void Stop()
        {
            _started = false;
            _journalWatcher.EnableRaisingEvents = false;
            // One last scan of live files
            List<IJournalEntry> entriesFound;
            lock (_monitoredFiles)
            {
                entriesFound = RescanFiles(_monitoredFiles.Values, LastRead).ToList();
            }
            ProcessEntries(entriesFound, BatchMode.Ongoing);
            _triggerUpdate.Dispose();
        }

        private void ProcessEntries(IList<IJournalEntry> journalEntries, BatchMode mode)
        {
            if (!journalEntries.Any()) return;

            JournalEntriesParsed?.Invoke(this, new JournalEntriesParsedEventArgs(journalEntries, mode));
            LastRead = journalEntries.OrderBy(x => x.Timestamp).Last().Timestamp;
        }

        private static IEnumerable<string> EnumerateFolder(string path, string pattern)
        {
            return Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly);
        }

        private IEnumerable<IJournalReader> FilterFilesByDate(IEnumerable<string> filePaths, DateTimeOffset? since)
        {
            return filePaths.Select(filePath =>
                {
                    var reader = _journalReaderFactory.Create(filePath);
                    var info = reader.FileInfo;

                    // If no header entry found, log is not a log file (yet).
                    // If it later gets a header, eventually we will be notified to start
                    // monitoring it, and we'll read it then.
                    if (!info.IsValid) return null;

                    return info.LastEntryTimeStamp > since.GetValueOrDefault(DateTime.MinValue) ? reader : null;
                })
                .Where(f => f != null)
                .OrderBy(f => f.FileInfo.LastEntryTimeStamp);
        }

        private IEnumerable<IJournalEntry> RescanFiles(IEnumerable<IJournalReader> readers, DateTimeOffset? since)
        {
            return readers.SelectMany(reader => RescanFile(reader, since));
        }

        private IEnumerable<IJournalEntry> RescanFile(IJournalReader reader, DateTimeOffset? since)
        {
            var info = reader.FileInfo;

            Log.Debug($"Scanning file {info.Path}");
            var count = 0;

            if (reader.FileExists)
            {
                
                foreach (var entry in reader.ReadAll(since))
                {
                    info.LastEntryTimeStamp = entry.Timestamp;
                    // Special case for 'Continued' journal entry - stop monitoring this file after this, as
                    // logging should be rolling over to a new file
                    if (entry is Continued)
                    {
                        StopMonitoringFile(info.Path);
                    }
                    count++;
                    yield return entry;
                }
            }
            else
            {
                StopMonitoringFile(info.Path);
            }

            if (count > 0)
            {
                Log.Info($"Scanned file {info.Path}, {count} new entries found");
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
                if (_monitoredFiles.ContainsKey(reader.FileInfo.Path)) return;
                _monitoredFiles.Add(reader.FileInfo.Path, reader);
            }
            JournalFileWatchingStarted?.Invoke(this, new JournalFileEventArgs(reader.FileInfo.Path));
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
            return LastRead;
        }
    }
}
