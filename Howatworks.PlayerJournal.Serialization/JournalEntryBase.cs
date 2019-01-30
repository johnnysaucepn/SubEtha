using System;

namespace Howatworks.PlayerJournal.Serialization
{
    public abstract class JournalEntryBase : IJournalEntry
    {
        public DateTimeOffset Timestamp { get; set; }
        public string GameVersionDiscriminator { get; set; }
        public string Event { get; set; }
    }
}
