using System;
using System.IO;
using log4net;

namespace Howatworks.PlayerJournal.Monitor
{
    /// <summary>
    /// The standard FileSystemWatcher class has several limitations, such as case-sensitivity (even on Windows).
    /// Also, for our needs, we don't generally care about the mechanics of renaming files, so abstract them away and build in useful logging.
    /// </summary>
    public class CustomFileWatcher
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomFileWatcher));

        private readonly string _filename;
        private readonly Action<string> _onCreated;
        private readonly Action<string> _onDeleted;
        private readonly FileSystemWatcher _journalWatcher;

        public CustomFileWatcher(string folder, string filename, Action<string> onCreated, Action<string> onDeleted)
        {
            _filename = filename;
            _onCreated = onCreated;
            _onDeleted = onDeleted;
            _journalWatcher = new FileSystemWatcher(folder)
            {
                EnableRaisingEvents = false
            };
            _journalWatcher.Renamed += JournalWatcher_OnRenamed;
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
            if (e.Name.Equals(_filename, StringComparison.InvariantCultureIgnoreCase))
            {
                LogWatcherEvent(e);
                _onDeleted(e.FullPath);
            }
        }

        private void JournalWatcher_OnCreated(object s, FileSystemEventArgs e)
        {
            if (e.Name.Equals(_filename, StringComparison.InvariantCultureIgnoreCase))
            {
                LogWatcherEvent(e);
                _onCreated(e.FullPath);
            }
        }

        private void JournalWatcher_OnRenamed(object s, RenamedEventArgs e)
        {
            // If renamed from something we care about
            if (e.OldName.Equals(_filename, StringComparison.InvariantCultureIgnoreCase))
            {
                LogWatcherEvent(e);
                _onDeleted(e.OldFullPath);
            }

            // If renamed to something we care about
            if (e.Name.Equals(_filename, StringComparison.InvariantCultureIgnoreCase))
            {
                LogWatcherEvent(e);
                _onCreated(e.FullPath);
            }
        }

        private static void LogWatcherEvent(FileSystemEventArgs e)
        {
            Log.Info($"Received {e.ChangeType} entry on file {e.FullPath}");
        }
    }
}