using System;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class MatrixJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixJournalProcessorPlugin));

        private readonly IConfiguration _pluginConfig;
        private readonly string _user;
        public FlushBehaviour FlushBehaviour => FlushBehaviour.OnEveryBatch;
        public CatchupBehaviour FirstRunBehaviour => CatchupBehaviour.Process;
        public CatchupBehaviour CatchupBehaviour => CatchupBehaviour.Process;

        public LocationManager Location { get; }
        public ShipManager Ship { get; set; }
        public SessionManager Session { get; set; }

        public MatrixJournalProcessorPlugin(IConfiguration config, LocationManager location, ShipManager ship, SessionManager session)
        {
            _pluginConfig = config.GetSection("Howatworks.Thumb.Plugin.Matrix");
            _user = _pluginConfig["User"];
            Location = location;
            Ship = ship;
            Session = session;
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

        /*public void Flush()
        {
            foreach (var game in _processors.Values)
            {
                game.BatchComplete();
            }
        }*/
    }
}