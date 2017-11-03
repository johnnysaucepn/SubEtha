using System;
using SubEtha.Domain;

namespace Thumb.Plugin.SubEtha
{
    public class LocationState : ILocationState
    {
        public DateTime TimeStamp { get; set; }

        public StarSystem StarSystem { get; set; }
        public Body Body { get; set; }
        public SurfaceLocation SurfaceLocation { get; set; }
        public Station Station { get; set; }
        public SignalSource SignalSource { get; set; }
    }
}
