using System.Collections.Generic;
using System.Linq;
using Howatworks.PlayerJournal.Monitor;
using Howatworks.PlayerJournal.Serialization;
using Thumb.Plugin;

namespace Thumb.Core
{
    public class ThumbProcessor
    {
        private readonly IList<IJournalProcessorPlugin> _plugins;

        public ThumbProcessor(IEnumerable<IJournalProcessorPlugin> plugins, IJournalMonitorNotifier notifier)
        {
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

                foreach (var journalEntry in entries)
                {
                    plugin.Apply(journalEntry);
                    // Upload on every entry if required
                    if (plugin.FlushBehaviour == FlushBehaviour.OnEveryAppliedEntry)
                    {
                        plugin.Flush();
                    }
                }

                if (plugin.FlushBehaviour == FlushBehaviour.OnEveryBatch)
                {
                    plugin.Flush();
                }
            }
        }


    }
}
