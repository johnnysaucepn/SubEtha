using System;

namespace Howatworks.PlayerJournal.Monitor
{
    public interface IJournalMonitorState
    {
        DateTimeOffset? LastRead { get; set; }
        DateTimeOffset? LastChecked { get; set; }
    }
}