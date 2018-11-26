using System;
using System.Collections.Generic;
using System.Diagnostics;
using Howatworks.PlayerJournal.Serialization;

namespace Howatworks.PlayerJournal.Parser
{
    public class JournalEntryRouter
    {
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

        public bool Apply<T>(T entry) where T : IJournalEntry
        {
            var t = entry.GetType();
            if (!_handlers.ContainsKey(t)) return false;

            var applied = false;
            foreach (var handler in _handlers[t])
            {
                Trace.TraceInformation($"Applying journal event {t.Name} to {GetType().Name}");
                if (!handler.Invoke(entry)) continue;
                applied = true;

            }
            return applied;
        }

        
    }
}
