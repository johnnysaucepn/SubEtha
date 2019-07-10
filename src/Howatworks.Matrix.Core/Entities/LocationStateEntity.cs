using System;
using Howatworks.Matrix.Domain;

namespace Howatworks.Matrix.Core.Entities
{
    public class LocationStateEntity : MatrixEntity, IGameContextEntity, ILocationState
    {
        public GameContext GameContext { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public Body Body { get; set; }
        public SignalSource SignalSource { get; set; }
        public StarSystem StarSystem { get; set; }
        public Station Station { get; set; }
        public SurfaceLocation SurfaceLocation { get; set; }
    }
}
