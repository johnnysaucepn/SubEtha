using System.Collections.Generic;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Monitor;
using log4net;
using Howatworks.Thumb.Plugin;

namespace Howatworks.Thumb.Core
{
    public class ThumbProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ThumbProcessor));

        private readonly JournalEntryRouter _router;

        public ThumbProcessor(IEnumerable<IJournalProcessorPlugin> plugins, JournalEntryRouter router)
        {
            _router = router;

            foreach (var plugin in plugins)
            {
                plugin.Startup();
            }
        }

        public void Apply(IEnumerable<IJournalEntry> entries, BatchMode mode)
        {
            /*foreach (var plugin in _plugins)
            {
                // Skip all existing log files on first run
                if (mode == BatchMode.FirstRun && plugin.FirstRunBehaviour == CatchupBehaviour.Skip) return;
                if (mode == BatchMode.Catchup && plugin.CatchupBehaviour == CatchupBehaviour.Skip) return;
            }*/

            foreach (var journalEntry in entries)
            {
                var somethingApplied = _router.Apply(journalEntry, mode);
                if (!somethingApplied)
                {
                    Log.Info($"No handler applied for event type {journalEntry.Event}");
                }
            }

            var batchProcessed = _router.ApplyBatchComplete(mode);
        }

    }
}
