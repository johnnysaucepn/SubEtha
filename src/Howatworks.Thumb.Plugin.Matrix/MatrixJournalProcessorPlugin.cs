using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class MatrixJournalProcessorPlugin : IJournalProcessorPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MatrixJournalProcessorPlugin));

        private readonly IConfiguration _pluginConfig;
        private readonly string _user;

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
    }
}