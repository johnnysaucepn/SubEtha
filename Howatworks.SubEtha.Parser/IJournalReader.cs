using System;
using System.Collections.Generic;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Parser
{
    public interface IJournalReader
    {
        string FilePath { get; }
        DateTimeOffset? LastEntryTimeStamp { get; }
        bool FileExists { get; }
        IEnumerable<IJournalEntry> ReadAll(DateTimeOffset since);
    }
}