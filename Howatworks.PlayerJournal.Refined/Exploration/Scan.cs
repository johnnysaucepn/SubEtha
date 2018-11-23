using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class Scan : JournalEntryBase
    {
        public class Ring
        {
            public string Name { get; set; }
            public string RingClass { get; set; }
            public decimal MassMT { get; set; }
            public decimal InnerRad { get; set; }
            public decimal OuterRad { get; set; }
        }

        #region Common to all planets/stars/moons
        public string BodyName { get; set; }
        public decimal DistanceFromArrivalLS { get; set; }
        public decimal SurfaceTemperature { get; set; }
        public Ring[] Rings { get; set; }
        #endregion


        #region Stars only
        public string StarType { get; set; }
        public decimal StellarMass { get; set; }
        public decimal Radius { get; set; }
        public decimal AbsoluteMagnitude { get; set; }
        public decimal RotationalPeriod { get; set; }
        public decimal Age_MY { get; set; }
        #endregion

        #region Planets and Moons only
        public bool TidalLock { get; set; }
        public string TerraformState { get; set; }
        public string PlanetClass { get; set; } // TODO: See 11.3
        public string Atmosphere { get; set;  } // TODO: See 11.4
        public string AtmosphereType { get; set; }
        public string Volcanism { get; set; } // TODO: See 11.5
        public decimal SurfaceGravity { get; set; }
        public decimal SurfacePressure { get; set; }
        public bool Landable { get; set; }
        public Dictionary<string, decimal> Materials { get; set; }
        public decimal RotationPeriod { get; set; }
        #endregion

        #region Orbital parameters
        public decimal SemiMajorAxis { get; set; }
        public decimal Eccentricity { get; set; }
        public decimal OrbitalInclination { get; set; }
        public decimal Periapsis { get; set; }
        public decimal OrbitalPeriod { get; set; }
        #endregion


    }
}

