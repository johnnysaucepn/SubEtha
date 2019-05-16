using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Exploration
{
    public class Scan : JournalEntryBase
    {
        public class MaterialItem
        {
            public string Name { get; set; }
            public decimal Percent { get; set; }
        }

        public class RingItem
        {
            public string Name { get; set; }
            public string RingClass { get; set; }
            public decimal MassMT { get; set; }
            public decimal InnerRad { get; set; }
            public decimal OuterRad { get; set; }
        }

        public class CompositionItem
        {
            public decimal Ice { get; set; }
            public decimal Rock { get; set; }
            public decimal Metal { get; set; }
        }

        public class AtmosphereCompositionItem
        {
            // WARNING: content is undocumented
            public string Name { get; set; }
            public decimal Percent { get; set; }
        }

        #region Common to all planets/stars/moons
        public string ScanType { get; set; } // TODO: consider enum  Basic, Detailed, NavBeacon, NavBeaconDetail, AutoScan
        public string BodyName { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; }
        public decimal DistanceFromArrivalLS { get; set; }
        public decimal Radius { get; set; } // WARNING: docs say star-only, definitely appears for planets
        public decimal SurfaceTemperature { get; set; }
        public List<RingItem> Rings { get; set; }
        public bool WasDiscovered { get; set; } // TODO: docs list this for stars only - suspect maybe for other bodies?
        public bool WasMapped { get; set; } // TODO: docs list this for stars only - suspect maybe for other bodies as stars can't be mapped?
        #endregion


        #region Stars only
        public string StarType { get; set; }
        public int Subclass { get; set; } // TODO: 0-9 - enum if there are names?
        public decimal? StellarMass { get; set; }
        public decimal? AbsoluteMagnitude { get; set; }
        public decimal? RotationalPeriod { get; set; }
        public decimal? Age_MY { get; set; }
        public string Luminosity { get; set; } // TODO: enum? // WARNING: not in docs
        #endregion

        #region Planets and Moons only
        public List<dynamic> Parents { get; set; } // TODO: spec suggests that key name in each pair varies
        public bool? TidalLock { get; set; } // WARNING: specs say int
        public string TerraformState { get; set; } // TODO: enum, Terraformable, Terraforming, Terraformed, or null
        public string PlanetClass { get; set; } // TODO: See 13.3
        public string Atmosphere { get; set;  } // TODO: See 13.4
        public string AtmosphereType { get; set; }
        public List<AtmosphereCompositionItem> AtmosphereComposition { get; set; }

        public string Volcanism { get; set; } // TODO: See 13.5
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public decimal? MassEM { get; set; } // WARNING: not in docs, in sample
        public decimal? SurfaceGravity { get; set; }
        public decimal? SurfacePressure { get; set; }
        public bool? Landable { get; set; }
        public List<MaterialItem> Materials { get; set; }
        public CompositionItem Composition { get; set; }
        public string ReserveLevel { get; set; } // TODO: check if applies to stars, enum: Pristine/Major/Common/Low/Depleted
        public decimal? RotationPeriod { get; set; }
        public decimal? AxialTilt { get; set; }
        #endregion

        #region Orbital parameters
        public decimal? SemiMajorAxis { get; set; }
        public decimal? Eccentricity { get; set; }
        public decimal? OrbitalInclination { get; set; }
        public decimal? Periapsis { get; set; }
        public decimal? OrbitalPeriod { get; set; }
        #endregion


    }

}

