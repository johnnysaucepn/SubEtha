using System.Collections.Generic;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class ControllerJournalProcessor : IJournalProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ControllerJournalProcessor));

        private readonly IList<IJournalProcessor> _childProcessors = new List<IJournalProcessor>();

        public ControllerJournalProcessor(IJournalMonitorNotifier notifier)
        {
            _childProcessors.Add(new StatusManager(notifier));
        }

        public bool Apply(IJournalEntry journalEntry)
        {
            var somethingApplied = false;
            Log.Debug(JsonConvert.SerializeObject(journalEntry));

            foreach (var manager in _childProcessors)
            {
                if (manager.Apply(journalEntry))
                {
                    somethingApplied = true;
                }
            }
            if (!somethingApplied)
            {
                Log.Info($"No handler applied for event type {journalEntry.Event}");
            }
            return somethingApplied;

        }

        public void Flush()
        {
            foreach (var manager in _childProcessors)
            {
                manager.Flush();
            }
        }
    }
}