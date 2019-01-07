using System;
using Howatworks.Configuration;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class ControllerJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ControllerJournalProcessorPlugin));

        private readonly IConfigReader _pluginConfig;
        private readonly ControllerJournalProcessor _processor;
        public FlushBehaviour FlushBehaviour { get; set; }
        public CatchupBehaviour FirstRunBehaviour { get; set; }
        public CatchupBehaviour CatchupBehaviour { get; set; }
       
        public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public ControllerJournalProcessorPlugin(IConfigReader sharedConfig, IConfigReader pluginConfig)
        {
            _pluginConfig = pluginConfig;
            _processor = new ControllerJournalProcessor();
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