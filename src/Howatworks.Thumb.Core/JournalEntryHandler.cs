using System;
using Howatworks.SubEtha.Journal;

namespace Howatworks.Thumb.Core
{
    public class JournalEntryHandler
    {
        public Func<dynamic, bool> Invoke { get; }
        public IBatchPolicy Policy { get; }

        private JournalEntryHandler(Func<dynamic, bool> func, IBatchPolicy policy)
        {
            Invoke = func;
            Policy = policy;
        }

        public static JournalEntryHandler Create<T>(Func<T, bool> invocation, IBatchPolicy policy) where T : IJournalEntry
        {
            return new JournalEntryHandler(x => invocation(x), policy);
        }
    }
}