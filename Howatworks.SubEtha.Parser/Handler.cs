using System;
using Howatworks.SubEtha.Journal;

namespace Howatworks.SubEtha.Parser
{
    public class Handler<T> : IHandler where T : IJournalEntry
    {
        private readonly Func<T, bool> _action;

        public Handler(Func<T, bool> action)
        {
            _action = action;
        }

        public bool Invoke(IJournalEntry journal, BatchMode mode)
        {
            return _action((T)journal);
        }
    }
}