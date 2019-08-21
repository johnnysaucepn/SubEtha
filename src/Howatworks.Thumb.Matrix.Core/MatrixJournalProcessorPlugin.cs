using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixJournalProcessorPlugin));

        private readonly IConfiguration _pluginConfig;

        public LocationManager Location { get; }
        public ShipManager Ship { get; set; }
        public SessionManager Session { get; set; }

        public MatrixJournalProcessorPlugin(IConfiguration config, LocationManager location, ShipManager ship, SessionManager session)
        {
            _pluginConfig = config.GetSection("Plugins:Howatworks.Thumb.Plugin.Matrix");
            Location = location;
            Ship = ship;
            Session = session;
        }

        public void Startup()
        {
        }
    }
}