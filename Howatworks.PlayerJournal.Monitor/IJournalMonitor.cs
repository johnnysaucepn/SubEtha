using System;
using System.Collections.Generic;
using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Monitor
{
    public interface IJournalMonitor
    {
        event EventHandler<JournalFileEventArgs> JournalFileWatchingStarted;
        event EventHandler<JournalFileEventArgs> JournalFileWatchingStopped;
        IList<IJournalEntry> Start(bool firstRun, DateTime lastRead);
        void Stop();
        IList<IJournalEntry> Update(DateTime lastRead);
    }
}