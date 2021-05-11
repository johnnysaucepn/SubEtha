using System;

namespace Howatworks.SubEtha.Journal
{
    public abstract class JournalEntryBase : IJournalEntry
    {
        public DateTimeOffset Timestamp { get; set; }
        public string Event { get; set; }
    }
}
