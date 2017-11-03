using System.Diagnostics.CodeAnalysis;

namespace SubEtha.Domain
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class SurfaceLocation
    {
        public SurfaceLocation()
        {
        }

        public SurfaceLocation(bool landed, decimal? latitude, decimal? longitude)
        {
            Landed = landed;
            Latitude = latitude;
            Longitude = longitude;
        }

        public bool Landed { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }
}