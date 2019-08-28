using System;

namespace Howatworks.SubEtha.Journal
{
    public interface IJournalEntry
    {
        string Event { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}