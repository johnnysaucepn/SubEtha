using System;
using System.IO;
using log4net;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Howatworks.SubEtha.Monitor
{
    /// <summary>
    /// The standard FileSystemWatcher class has several limitations, such as case-sensitivity (even on Windows).
    /// Also, for our needs, we don't generally care about the mechanics of renaming files, so abstract them away and build in useful logging.
    /// </summary>
    public class CustomFileWatcher
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomFileWatcher));

        public event Action<string> Created;
        public event Action<string> Changed;
        public event Action<string> Deleted;
        private readonly FileSystemWatcher _journalWatcher;
        private readonly Matcher _matcher;

        public CustomFileWatcher(string folder, string pattern)
        {
            _matcher = new Matcher(StringComparison.InvariantCultureIgnoreCase).AddInclude(pattern);
            _journalWatcher = new FileSystemWatcher(folder)
            {
                EnableRaisingEvents = false
            };
            _journalWatcher.Renamed += JournalWatcher_OnRenamed;
            _journalWatcher.Changed += JournalWatcher_OnChanged;
            _journalWatcher.Created += JournalWatcher_OnCreated;
            _journalWatcher.Deleted += JournalWatcher_OnDeleted;
        }

        public void Start()
        {
            _journalWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _journalWatcher.EnableRaisingEvents = false;
        }

        private void JournalWatcher_OnDeleted(object s, FileSystemEventArgs e)
        {
            if (_matcher.Match(e.Name).HasMatches)
            {
                LogWatcherEvent(e);
                Deleted?.Invoke(e.FullPath);
            }
        }

        private void JournalWatcher_OnChanged(object s, FileSystemEventArgs e)
        {
            if (_matcher.Match(e.Name).HasMatches)
            {
                LogWatcherEvent(e);
                Changed?.Invoke(e.FullPath);
            }
        }

        private void JournalWatcher_OnCreated(object s, FileSystemEventArgs e)
        {
            if (_matcher.Match(e.Name).HasMatches)
            {
                LogWatcherEvent(e);
                Created?.Invoke(e.FullPath);
            }
        }

        private void JournalWatcher_OnRenamed(object s, RenamedEventArgs e)
        {
            // If renamed from something we care about
            if (_matcher.Match(e.OldName).HasMatches)
            {
                LogWatcherEvent(e);
                Deleted?.Invoke(e.OldFullPath);
            }

            // If renamed to something we care about
            if (_matcher.Match(e.Name).HasMatches)
            {
                LogWatcherEvent(e);
                Created?.Invoke(e.FullPath);
            }
        }

        private static void LogWatcherEvent(FileSystemEventArgs e)
        {
            Log.Info($"Received {e.ChangeType} entry on file {e.FullPath}");
        }
    }
}