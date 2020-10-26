using System;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.Matrix.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class SurfaceLocation : IEquatable<SurfaceLocation>
    {
        public bool Landed { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }

        public SurfaceLocation()
        {
        }

        public SurfaceLocation(bool landed, decimal? latitude, decimal? longitude)
        {
            Landed = landed;
            Latitude = latitude;
            Longitude = longitude;
        }

        public bool Equals(SurfaceLocation other)
        {
            if (other == null) return false;

            if (Landed != other.Landed) return false;
            if (Longitude != other.Longitude) return false;
            if (Latitude != other.Latitude) return false;

            return true;
        }
    }
}