using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Howatworks.Configuration;
using Howatworks.PlayerJournal;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Processing;
using Newtonsoft.Json;

namespace Thumb.Plugin.SubEtha
{
    public class SubEthaJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private readonly IConfigReader _sharedConfig;
        private readonly IConfigReader _pluginConfig;
        private readonly IDictionary<string, SubEthaJournalProcessor> _processors = new Dictionary<string, SubEthaJournalProcessor>();
        private readonly string _user;
        private readonly FlushBehaviour _flushBehaviour;
        private readonly CatchupBehaviour _firstRunBehaviour;
        private readonly CatchupBehaviour _catchupBehaviour;

        public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public SubEthaJournalProcessorPlugin(IConfigReader sharedConfig, IConfigReader pluginConfig)
        {
            _sharedConfig = sharedConfig;
            _pluginConfig = pluginConfig;
            _user = _sharedConfig.Get<string>("User");
            _flushBehaviour = FlushBehaviour.OnEveryBatch;
            _firstRunBehaviour = CatchupBehaviour.Process;
            _catchupBehaviour = CatchupBehaviour.Process;
        }

        public void Apply(IEnumerable<JournalEntryBase> entries, BatchMode mode)
        {
            // TODO: extract this logic into a handling class
            // Skip all existing log files on first run
            if (mode == BatchMode.FirstRun && _firstRunBehaviour == CatchupBehaviour.Skip) return;
            if (mode == BatchMode.Catchup && _catchupBehaviour == CatchupBehaviour.Skip) return;

            foreach (var entry in entries)
            {
                var gameVersion = entry.GameVersionDiscriminator;

                if (!_processors.ContainsKey(gameVersion))
                {
                    _processors[gameVersion] = new SubEthaJournalProcessor(_pluginConfig, _user, gameVersion);
                }
                var game = _processors[gameVersion];
                Debug.WriteLine(JsonConvert.SerializeObject(entry));

                game.Apply(entry);
                AppliedJournalEntries?.Invoke(this, new AppliedJournalEntriesEventArgs());

                // Upload on every entry if required
                if (_flushBehaviour == FlushBehaviour.OnEveryAppliedEntry)
                {
                    game.Flush();
                    FlushedJournalProcessor?.Invoke(this, new FlushedJournalProcessorEventArgs());
                }
            }
            if (_flushBehaviour == FlushBehaviour.OnEveryBatch)
            {
                foreach (var game in _processors.Keys.Select(gameVersion => _processors[gameVersion]))
                {
                    game.Flush();
                    FlushedJournalProcessor?.Invoke(this, new FlushedJournalProcessorEventArgs());
                }
            }
        }
    }
}