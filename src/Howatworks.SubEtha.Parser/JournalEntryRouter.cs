using System;
using System.Collections.Generic;
using Howatworks.SubEtha.Journal;
using log4net;

namespace Howatworks.SubEtha.Parser
{
    public class JournalEntryRouter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalEntryRouter));

        private readonly Dictionary<Type, List<IHandler>> _handlers = new Dictionary<Type, List<IHandler>>();

        public void RegisterFor<T>(Func<T, bool> handler) where T : IJournalEntry
        {
            var t = typeof(T);
            if (!_handlers.ContainsKey(t))
            {
                _handlers[t] = new List<IHandler>();
            }
            _handlers[t].Add(new Handler<T>(handler));
        }

        public bool Apply<T>(T entry, BatchMode mode) where T : IJournalEntry
        {
            var t = entry.GetType();
            if (!_handlers.ContainsKey(t)) return false;

            var applied = false;
            foreach (var handler in _handlers[t])
            {
                Log.Info($"Applying journal event {t.Name} to {GetType().Name}");
                if (!handler.Invoke(entry, mode)) continue;
                applied = true;

            }
            return applied;
        }


    }
}
