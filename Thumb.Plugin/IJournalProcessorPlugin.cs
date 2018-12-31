using System;
using Howatworks.PlayerJournal.Serialization;

namespace Thumb.Plugin
{
    public interface IJournalProcessorPlugin
    {
        event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;
        FlushBehaviour FlushBehaviour { get; set; }
        CatchupBehaviour FirstRunBehaviour { get; set; }
        CatchupBehaviour CatchupBehaviour { get; set; }
        void Apply(IJournalEntry journalEntry);
        void Flush();
    }
}
