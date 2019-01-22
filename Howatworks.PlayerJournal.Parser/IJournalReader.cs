using System;
using System.Collections.Generic;
using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalReader
    {
        string FilePath { get; }
        DateTimeOffset? LastEntryTimeStamp { get; }
        bool FileExists { get; }
        IEnumerable<IJournalEntry> ReadAll(DateTimeOffset since);
    }
}