using System;
using System.Collections.Generic;
using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Parser
{
    public interface IJournalReader
    {
        JournalFileInfo FileInfo { get; }
        bool FileExists { get; }
        IEnumerable<IJournalEntry> ReadAll(DateTimeOffset? since);
    }
}