using System;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class ControllerJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ControllerJournalProcessorPlugin));

        private readonly ControllerJournalProcessor _processor;
        public FlushBehaviour FlushBehaviour => FlushBehaviour.OnEveryBatch;
        public CatchupBehaviour FirstRunBehaviour => CatchupBehaviour.Skip;
        public CatchupBehaviour CatchupBehaviour => CatchupBehaviour.Skip;

        public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public ControllerJournalProcessorPlugin(IConfiguration configuration, IJournalMonitorNotifier notifier)
        {
            _processor = new ControllerJournalProcessor(notifier);
        }

        public void Apply(IJournalEntry journalEntry)
        {
            Log.Debug(JsonConvert.SerializeObject(journalEntry));

            _processor.Apply(journalEntry);
            AppliedJournalEntries?.Invoke(this, new AppliedJournalEntriesEventArgs());
        }

        public void Flush()
        {
            _processor.Flush();
            FlushedJournalProcessor?.Invoke(this, new FlushedJournalProcessorEventArgs());
        }
    }
}