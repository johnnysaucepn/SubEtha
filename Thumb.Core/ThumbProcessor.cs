using System.Collections.Generic;
using System.Linq;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Thumb.Plugin;

namespace Thumb.Core
{
    public class ThumbProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ThumbProcessor));

        private readonly JournalEntryRouter _router;
        private readonly IList<IJournalProcessorPlugin> _plugins;

        public ThumbProcessor(IEnumerable<IJournalProcessorPlugin> plugins, IJournalMonitorNotifier notifier, JournalEntryRouter router)
        {
            _router = router;
            _plugins = plugins.ToList();

            foreach (var plugin in _plugins)
            {
                plugin.Startup();
                plugin.FlushedJournalProcessor += (sender, args) => { notifier.UpdatedService(sender); };
            }
        }

        public void Apply(IList<IJournalEntry> entries, BatchMode mode)
        {
            foreach (var plugin in _plugins)
            {
                // Skip all existing log files on first run
                if (mode == BatchMode.FirstRun && plugin.FirstRunBehaviour == CatchupBehaviour.Skip) return;
                if (mode == BatchMode.Catchup && plugin.CatchupBehaviour == CatchupBehaviour.Skip) return;
            }

            foreach (var journalEntry in entries)
            {
                var somethingApplied = _router.Apply(journalEntry, mode);
                if (!somethingApplied)
                {
                    Log.Info($"No handler applied for event type {journalEntry.Event}");
                }

                // Upload on every entry if required
                foreach (var plugin in _plugins)
                {
                    if (plugin.FlushBehaviour == FlushBehaviour.OnEveryAppliedEntry)
                    {
                        plugin.Flush();
                    }
                }
            }

            foreach (var plugin in _plugins)
            {
                if (plugin.FlushBehaviour == FlushBehaviour.OnEveryBatch)
                {
                    plugin.Flush();
                }
            }
        }

    }
}
