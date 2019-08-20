using System.Collections.Generic;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Monitor;
using log4net;

namespace Howatworks.Thumb.Core
{
    public class ThumbProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ThumbProcessor));

        private readonly IEnumerable<IJournalProcessorPlugin> _plugins;
        private readonly JournalEntryRouter _router;

        public ThumbProcessor(IEnumerable<IJournalProcessorPlugin> plugins, JournalEntryRouter router)
        {
            _plugins = plugins;
            _router = router;
        }

        public void Startup()
        {
            foreach (var plugin in _plugins)
            {
                plugin.Startup();
            }
        }

        public void Apply(IEnumerable<IJournalEntry> entries, BatchMode mode)
        {
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
