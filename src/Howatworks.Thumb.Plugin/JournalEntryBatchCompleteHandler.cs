using System;

namespace Howatworks.Thumb.Plugin
{
    public class JournalEntryBatchCompleteHandler
    {
        public Func<bool> Invoke { get; }
        public IBatchPolicy Policy { get; }

        public JournalEntryBatchCompleteHandler(Func<bool> func, IBatchPolicy policy)
        {
            Invoke = func;
            Policy = policy;
        }
    }
}