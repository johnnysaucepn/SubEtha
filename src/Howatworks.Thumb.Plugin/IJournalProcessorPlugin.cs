using System;

namespace Howatworks.Thumb.Plugin
{
    public interface IJournalProcessorPlugin
    {
        CatchupBehaviour FirstRunBehaviour { get; }
        CatchupBehaviour CatchupBehaviour { get; }

        void Startup();
    }
}
