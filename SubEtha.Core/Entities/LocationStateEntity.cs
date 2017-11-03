using System;
using SubEtha.Domain;

namespace SubEtha.Core.Entities
{
    public class LocationStateEntity : IEntity, IGameContextEntity, ILocationState
    {
        public Guid Id { get; set; }

        public GameContext GameContext { get; set; }

        public DateTime TimeStamp { get; set; }

        public Body Body { get; set; }
        public SignalSource SignalSource { get; set; }
        public StarSystem StarSystem { get; set; }
        public Station Station { get; set; }
        public SurfaceLocation SurfaceLocation { get; set; }
    }
}
