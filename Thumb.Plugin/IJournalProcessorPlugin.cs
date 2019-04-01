using System;

namespace Thumb.Plugin
{
    public interface IJournalProcessorPlugin
    {
        event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;
        FlushBehaviour FlushBehaviour { get; }
        CatchupBehaviour FirstRunBehaviour { get; }
        CatchupBehaviour CatchupBehaviour { get; }

        void Startup();
        void Flush();
    }
}
