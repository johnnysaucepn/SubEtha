using System;
using System.Collections.Generic;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Processing;
using Howatworks.PlayerJournal.Serialization;

namespace Thumb.Plugin
{
    public interface IJournalProcessorPlugin
    {
        void Apply(IEnumerable<IJournalEntry> entries, BatchMode mode);
        event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;
    }
}
