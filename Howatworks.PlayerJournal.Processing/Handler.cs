using System;

namespace Howatworks.PlayerJournal.Processing
{
    public class Handler<T> : IHandler where T : JournalEntryBase
    {
        private readonly Func<T, bool> _action;

        public Handler(Func<T, bool> action)
        {
            _action = action;
        }

        public bool Invoke(JournalEntryBase journal)
        {
            return _action((T)journal);
        }
    }
}