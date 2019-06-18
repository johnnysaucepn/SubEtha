using System;
using System.Collections.Generic;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Monitor;
using log4net;

namespace Howatworks.Thumb.Core
{
    public class JournalEntryRouter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JournalEntryRouter));

        public delegate bool JournalEntryHandler<in T>(T journal, BatchMode mode);
        public delegate bool JournalEntryBatchComplete(BatchMode mode);

        private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();
        private readonly List<JournalEntryBatchComplete> _batchComplete = new List<JournalEntryBatchComplete>();

        public void RegisterFor<T>(JournalEntryHandler<T> handler) where T : IJournalEntry
        {
            var t = typeof(T);
            if (!_handlers.ContainsKey(t))
            {
                var newHandlerList = new List<Delegate>();
                _handlers[t] = newHandlerList;
            }
            _handlers[t].Add(handler);
        }

        public void RegisterEndBatch(JournalEntryBatchComplete handler)
        {
            _batchComplete.Add(handler);
        }

        public bool Apply<T>(T entry, BatchMode mode) where T : IJournalEntry
        {
            var t = entry.GetType();
            if (!_handlers.ContainsKey(t)) return false;

            var applied = false;
            foreach (var handler in _handlers[t])
            {
                Log.Info($"Applying journal event {t.Name} to {GetType().Name}");
                var thisApplied = (bool)handler.DynamicInvoke(entry, mode);
                applied = applied || thisApplied;

            }
            return applied;
        }

        public bool BatchComplete(BatchMode mode)
        {
            var applied = false;
            foreach (var handler in _batchComplete)
            {
                if (!handler.Invoke(mode)) continue;
                applied = true;
            }
            return applied;
        }


    }
}
