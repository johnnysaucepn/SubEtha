using System;

namespace Howatworks.Matrix.Domain
{
    public class LocationState : ILocationState
    {
        public DateTimeOffset TimeStamp { get; set; }

        public StarSystem StarSystem { get; set; }
        public Body Body { get; set; }
        public SurfaceLocation SurfaceLocation { get; set; }
        public Station Station { get; set; }
        public SignalSource SignalSource { get; set; }
    }
}
