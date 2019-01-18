using System;

namespace Howatworks.PlayerJournal.Monitor
{
    public interface IJournalMonitorState
    {
        DateTime? LastRead { get; set; }
    }
}