using System;

namespace Howatworks.PlayerJournal.Monitor
{
    public interface IJournalMonitor
    {
        event EventHandler<JournalEntriesParsedEventArgs> JournalEntriesParsed;
        event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;
        void Start();
        void Stop();
        DateTime? LastUpdated();
    }
}