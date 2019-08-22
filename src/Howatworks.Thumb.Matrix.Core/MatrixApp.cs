using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixApp
    {
        public LocationManager Location { get; }
        public ShipManager Ship { get; set; }
        public SessionManager Session { get; set; }

        public MatrixApp(LocationManager location, ShipManager ship, SessionManager session, IConfiguration config2)
        {
            Location = location;
            Ship = ship;
            Session = session;
        }

        public void Startup()
        {
        }
    }
}