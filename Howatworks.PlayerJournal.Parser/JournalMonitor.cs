using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Howatworks.PlayerJournal.Other;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalMonitor : IDisposable
    {
        private readonly IJournalParser _parser;
        private readonly IJournalMonitorConfiguration _config;
        private readonly FileSystemWatcher _watcher;

        public event EventHandler<JournalEntriesParsedEventArgs> JournalEntriesParsed;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        public event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;

        private DateTime? LastRead
        {
            get { return _config.LastRead; }
            set { _config.LastRead = value; }
        }

        private readonly Timer _triggerUpdate;
        private readonly Dictionary<string, JournalReader> _monitoredFiles = new Dictionary<string, JournalReader>();

        public JournalMonitor(IJournalMonitorConfiguration config, IJournalParser parser)
        {
            _config = config;
            _parser = parser;

            _watcher = new FileSystemWatcher(_config.JournalFolder, _config.JournalPattern)
            {
                EnableRaisingEvents = false
            };
            _watcher.Created += (s, e) => { LogWatcherEvent(e); StartMonitoringFile(e.FullPath); };
            _watcher.Changed += (s, e) => { LogWatcherEvent(e); StartMonitoringFile(e.FullPath); };

            var updatePeriod = Convert.ToInt32(_config.UpdateInterval.TotalMilliseconds);
            _triggerUpdate = new Timer(TriggerUpdate_Elapsed, null, updatePeriod, updatePeriod);
        }

        private void TriggerUpdate_Elapsed(object data)
        {
            lock (_monitoredFiles)
            {
                if (_monitoredFiles.Count <= 0) return;

                Debug.WriteLine($"Rescanning {_monitoredFiles.Count} log files...");
                
                var entriesFound = RescanFiles(_monitoredFiles.Values, LastRead).ToList();
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

            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            // One last scan of live files
            List<JournalEntryBase> entriesFound;
            lock (_monitoredFiles)
            {
                entriesFound = RescanFiles(_monitoredFiles.Values, LastRead).ToList();
            }
            ProcessEntries(entriesFound, BatchMode.Ongoing);
            _triggerUpdate.Dispose();
        }

        private void ProcessEntries(IList<JournalEntryBase> entries, BatchMode mode)
        {
            if (!entries.Any()) return;

            JournalEntriesParsed?.Invoke(this, new JournalEntriesParsedEventArgs(entries, mode));
            LastRead = entries.OrderBy(x => x.TimeStamp).Last().TimeStamp;
        }

        private static IEnumerable<string> EnumerateFolder(string path, string pattern)
        {
            return Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly);
        }

        private IEnumerable<JournalReader> FilterFilesByDate(IEnumerable<string> filePaths, DateTime? since)
        {
            return filePaths.Select(filePath =>
            {
                var reader = new JournalReader(filePath, _parser);
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

        private IEnumerable<JournalEntryBase> RescanFiles(IEnumerable<JournalReader> readers, DateTime? since)
        {
            return readers.SelectMany(reader => RescanFile(reader, since));
        }

        private IEnumerable<JournalEntryBase> RescanFile(JournalReader reader, DateTime? since)
        {
            var info = reader.FileInfo;

            Debug.WriteLine($"Scanning file {info.Path}");
            var count = 0;

            if (reader.FileExists)
            {
                
                foreach (var entry in reader.ReadAll(since))
                {
                    info.LastEntryTimeStamp = entry.TimeStamp;
                    // Special case for 'Continued' journal event - stop monitoring this file after this, as
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
                Trace.TraceInformation($"Scanned file {info.Path}, {count} new entries found");
            }
        }

        private static void LogWatcherEvent(FileSystemEventArgs e)
        {
            Trace.TraceInformation($"Received {e.ChangeType} event on file {e.FullPath}");
        }

        private void StartMonitoringFile(string path)
        {
            StartMonitoringFile(new JournalReader(path, _parser));
            //TODO: should check validity of JournalReader before adding it
        }

        private void StartMonitoringFile(JournalReader reader)
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
                    Trace.TraceError($"Not monitoring file {path} - cannot stop");
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
