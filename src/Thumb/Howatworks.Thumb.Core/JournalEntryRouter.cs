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

        private readonly Dictionary<Type, List<JournalEntryHandler>> _journalHandlers = new Dictionary<Type, List<JournalEntryHandler>>();
        private readonly List<JournalEntryBatchCompleteHandler> _batchHandlers = new List<JournalEntryBatchCompleteHandler>();

        public void RegisterFor<T>(Func<T, bool> action, IBatchPolicy policy = null) where T : IJournalEntry
        {
            policy = policy ?? BatchPolicy.All;

            var handler = JournalEntryHandler.Create(action, policy);
            var t = typeof(T);
            if (!_journalHandlers.ContainsKey(t))
            {
                _journalHandlers[t] = new List<JournalEntryHandler>();
            }
            _journalHandlers[t].Add(handler);
        }

        public void RegisterForBatchComplete(Func<bool> action, IBatchPolicy policy = null)
        {
            policy = policy ?? BatchPolicy.All;

            _batchHandlers.Add(new JournalEntryBatchCompleteHandler(action, policy));
        }

        public void Apply(IEnumerable<IJournalEntry> entries, BatchMode mode)
        {
            foreach (var journalEntry in entries)
            {
                var somethingApplied = Apply(journalEntry, mode);
                if (!somethingApplied)
                {
                    Log.Info($"No handler applied for event type {journalEntry.Event}");
                }
            }

            var batchProcessed = ApplyBatchComplete(mode);
        }

        private bool Apply<T>(T entry, BatchMode mode) where T : IJournalEntry
        {
            var t = entry.GetType();
            if (!_journalHandlers.ContainsKey(t)) return false;

            var applied = false;
            Log.Info($"Applying journal event {t.Name}");
            foreach (var handler in _journalHandlers[t])
            {
                if (!handler.Policy.Accepts(mode)) continue;

                applied = handler.Invoke(entry) || applied;
            }
            return applied;
        }

        private bool ApplyBatchComplete(BatchMode mode)
        {
            var applied = false;
            Log.Info("Applying end of batch");
            foreach (var handler in _batchHandlers)
            {
                if (!handler.Policy.Accepts(mode)) continue;

                applied = applied || handler.Invoke();
            }
            return applied;
        }
    }
}
