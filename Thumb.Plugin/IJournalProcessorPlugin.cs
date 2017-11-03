using System;
using System.Collections.Generic;
using Howatworks.PlayerJournal;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Processing;

namespace Thumb.Plugin
{
    public interface IJournalProcessorPlugin
    {
        void Apply(IEnumerable<JournalEntryBase> entries, BatchMode mode);
        event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;
    }
}
