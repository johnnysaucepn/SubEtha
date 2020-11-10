using System;

namespace Howatworks.Matrix.Domain
{
    public class LocationState : ILocationState, ICloneable<LocationState>, IStateComparable<LocationState>
    {
        public DateTimeOffset TimeStamp { get; set; }

        public StarSystem StarSystem { get; set; }
        public Body Body { get; set; }
        public SurfaceLocation SurfaceLocation { get; set; }
        public Station Station { get; set; }
        public SignalSource SignalSource { get; set; }

        public LocationState Clone()
        {
            return new LocationState
            {
                TimeStamp = this.TimeStamp,
                StarSystem = this.StarSystem,
                Body = this.Body,
                SurfaceLocation = this.SurfaceLocation,
                Station = this.Station,
                SignalSource = this.SignalSource
            };
        }

        public bool HasChangedSince(LocationState state)
        {
            if (state == null) return false;

            if (!SimpleComparer.Equals(StarSystem, state.StarSystem)) return true;
            if (!SimpleComparer.Equals(Body, state.Body)) return true;
            if (!SimpleComparer.Equals(SurfaceLocation, state.SurfaceLocation)) return true;
            if (!SimpleComparer.Equals(Station, state.Station)) return true;
            if (!SimpleComparer.Equals(SignalSource, state.SignalSource)) return true;

            return false;
        }
    }
}
