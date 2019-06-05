using System;

namespace Howatworks.SubEtha.Journal
{
    public interface IJournalEntry
    {
        string Event { get; set; }
        string GameVersionDiscriminator { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}