using System;
using Howatworks.PlayerJournal.Serialization;

namespace Thumb.Plugin
{
    public interface IJournalProcessorPlugin
    {
        event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;
        FlushBehaviour FlushBehaviour { get; }
        CatchupBehaviour FirstRunBehaviour { get; }
        CatchupBehaviour CatchupBehaviour { get; }

        void Startup();
        void Apply(IJournalEntry journalEntry);
        void Flush();
    }
}
