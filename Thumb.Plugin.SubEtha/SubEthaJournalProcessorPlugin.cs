using System;
using System.Collections.Generic;
using Howatworks.PlayerJournal.Serialization;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Thumb.Plugin.SubEtha
{
    public class SubEthaJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SubEthaJournalProcessorPlugin));

        private readonly IConfiguration _pluginConfig;
        private readonly string _user;
        public FlushBehaviour FlushBehaviour => FlushBehaviour.OnEveryBatch;
        public CatchupBehaviour FirstRunBehaviour => CatchupBehaviour.Process;
        public CatchupBehaviour CatchupBehaviour => CatchupBehaviour.Process;

        private readonly IDictionary<string, IJournalProcessor> _processors = new Dictionary<string, IJournalProcessor>();

        public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public SubEthaJournalProcessorPlugin(IConfiguration sharedConfig, IConfiguration pluginConfig)
        {
            _user = sharedConfig["User"];
            _pluginConfig = pluginConfig;
        }

        public void Apply(IJournalEntry journalEntry)
        {
            var gameVersion = journalEntry.GameVersionDiscriminator;

            if (!_processors.ContainsKey(gameVersion))
            {
                _processors[gameVersion] = new SubEthaJournalProcessor(_pluginConfig, _user, gameVersion);
            }
            var game = _processors[gameVersion];
            Log.Debug(JsonConvert.SerializeObject(journalEntry));

            game.Apply(journalEntry);
            AppliedJournalEntries?.Invoke(this, new AppliedJournalEntriesEventArgs());
        }

        public void Flush()
        {
            foreach (var game in _processors.Values)
            {
                game.Flush();
                FlushedJournalProcessor?.Invoke(this, new FlushedJournalProcessorEventArgs());
            }
        }
    }
}