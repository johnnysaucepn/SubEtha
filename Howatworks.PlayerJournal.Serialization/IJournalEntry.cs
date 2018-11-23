using System;

namespace Howatworks.PlayerJournal.Serialization
{
    public interface IJournalEntry
    {
        string Event { get; set; }
        string GameVersionDiscriminator { get; set; }
        DateTime Timestamp { get; set; }
    }
}