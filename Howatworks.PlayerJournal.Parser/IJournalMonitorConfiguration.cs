using System;

namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalMonitorConfiguration
    {
        string JournalPattern { get; }
        string JournalFolder { get; }
        TimeSpan UpdateInterval { get; }
        DateTime? LastRead { get; set; }
    }
}