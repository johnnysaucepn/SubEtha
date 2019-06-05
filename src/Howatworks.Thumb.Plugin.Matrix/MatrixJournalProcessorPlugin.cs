using System;
using System.Collections.Generic;
using Howatworks.SubEtha.Parser;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class MatrixJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixJournalProcessorPlugin));

        private readonly IConfiguration _pluginConfig;
        private readonly JournalEntryRouter _router;
        private readonly string _user;
        public FlushBehaviour FlushBehaviour => FlushBehaviour.OnEveryBatch;
        public CatchupBehaviour FirstRunBehaviour => CatchupBehaviour.Process;
        public CatchupBehaviour CatchupBehaviour => CatchupBehaviour.Process;

        private readonly IDictionary<string, IJournalProcessor> _processors = new Dictionary<string, IJournalProcessor>();

        //public event EventHandler<AppliedJournalEntriesEventArgs> AppliedJournalEntries;
        public event EventHandler<FlushedJournalProcessorEventArgs> FlushedJournalProcessor;

        public MatrixJournalProcessorPlugin(IConfiguration config, JournalEntryRouter router)
        {
            _pluginConfig = config.GetSection("Howatworks.Thumb.Plugin.Matrix");
            _user = _pluginConfig["User"];
            _router = router;
        }

        public void Startup()
        {
        }

        /*public void Apply(IJournalEntry journalEntry)
        {
            var gameVersion = journalEntry.GameVersionDiscriminator;

            if (!_processors.ContainsKey(gameVersion))
            {
                _processors[gameVersion] = new MatrixJournalProcessor(_pluginConfig, _user, gameVersion, _router);
            }
            var game = _processors[gameVersion];
            Log.Debug(JsonConvert.SerializeObject(journalEntry));

            //game.Apply(journalEntry);
            AppliedJournalEntries?.Invoke(this, new AppliedJournalEntriesEventArgs());
        }*/

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