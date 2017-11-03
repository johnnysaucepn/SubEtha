using System;
using System.Collections.Generic;
using Common.Logging;

namespace Howatworks.PlayerJournal.Processing
{
    public class JournalEntryRouter
    {
        private static readonly ILog Log = LogManager.GetLogger<JournalEntryRouter>();

        private readonly Dictionary<Type, List<IHandler>> _handlers = new Dictionary<Type, List<IHandler>>();

        public void RegisterFor<T>(Func<T, bool> handler) where T : JournalEntryBase
        {
            var t = typeof(T);
            if (!_handlers.ContainsKey(t))
            {
                _handlers[t] = new List<IHandler>();
            }
            _handlers[t].Add(new Handler<T>(handler));
        }

        public bool Apply<T>(T entry) where T : JournalEntryBase
        {
            var t = entry.GetType();
            if (!_handlers.ContainsKey(t)) return false;

            var applied = false;
            foreach (var handler in _handlers[t])
            {
                Log.Info($"Applying journal event {t.Name} to {GetType().Name}");
                if (!handler.Invoke(entry)) continue;
                applied = true;

            }
            return applied;
        }

        
    }
}
