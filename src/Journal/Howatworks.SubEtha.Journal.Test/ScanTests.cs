using Howatworks.SubEtha.Journal.Exploration;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class ScanTests
    {
        private const string AutoStarSample = @"{ ""timestamp"":""2019-05-01T00:51:25Z"", ""event"":""Scan"", ""ScanType"":""AutoScan"", ""BodyName"":""Core Sys Sector ON-T b3-5 A"", ""BodyID"":2, ""Parents"":[ {""Null"":1}, {""Null"":0} ], ""DistanceFromArrivalLS"":0.000000, ""StarType"":""M"", ""Subclass"":3, ""StellarMass"":0.359375, ""Radius"":342825792.000000, ""AbsoluteMagnitude"":9.078339, ""Age_MY"":9610, ""SurfaceTemperature"":3095.000000, ""Luminosity"":""Va"", ""SemiMajorAxis"":48357699584.000000, ""Eccentricity"":0.208623, ""OrbitalInclination"":-8.817607, ""Periapsis"":96.832062, ""OrbitalPeriod"":35633192.000000, ""RotationPeriod"":159166.468750, ""AxialTilt"":0.000000, ""WasDiscovered"":true, ""WasMapped"":false }";
        private const string DetailedStarSample = @"{ ""timestamp"":""2019-05-01T00:54:55Z"", ""event"":""Scan"", ""ScanType"":""Detailed"", ""BodyName"":""Kipsigines B"", ""BodyID"":2, ""Parents"":[ {""Null"":0} ], ""DistanceFromArrivalLS"":2892.031982, ""StarType"":""L"", ""Subclass"":5, ""StellarMass"":0.125000, ""Radius"":173132112.000000, ""AbsoluteMagnitude"":13.377014, ""Age_MY"":11096, ""SurfaceTemperature"":1619.000000, ""Luminosity"":""V"", ""SemiMajorAxis"":516555669504.000000, ""Eccentricity"":0.350433, ""OrbitalInclination"":-10.644156, ""Periapsis"":248.491974, ""OrbitalPeriod"":623837760.000000, ""RotationPeriod"":85390.703125, ""AxialTilt"":0.000000, ""Rings"":[ { ""Name"":""Kipsigines B A Belt"", ""RingClass"":""eRingClass_Rocky"", ""MassMT"":2.1301e+12, ""InnerRad"":2.8567e+08, ""OuterRad"":1.2395e+09 } ], ""WasDiscovered"":true, ""WasMapped"":false }";

        private const string AutoPlanetSample = @"{ ""timestamp"":""2018-12-12T23:21:12Z"", ""event"":""Scan"", ""ScanType"":""AutoScan"", ""BodyName"":""HIP 32328 1"", ""BodyID"":1, ""Parents"":[ {""Star"":0} ], ""DistanceFromArrivalLS"":16.256958, ""TidalLock"":true, ""TerraformState"":"""", ""PlanetClass"":""Metal rich body"", ""Atmosphere"":""hot thick silicate vapour atmosphere"", ""AtmosphereType"":""SilicateVapour"", ""AtmosphereComposition"":[ { ""Name"":""Silicates"", ""Percent"":99.999962 } ], ""Volcanism"":""major silicate vapour geysers volcanism"", ""MassEM"":1.652256, ""Radius"":5601487.000000, ""SurfaceGravity"":20.988426, ""SurfaceTemperature"":4307.923828, ""SurfacePressure"":8932208640.000000, ""Landable"":false, ""Composition"":{ ""Ice"":0.000000, ""Rock"":0.000000, ""Metal"":1.000000 }, ""SemiMajorAxis"":4873715200.000000, ""Eccentricity"":0.000006, ""OrbitalInclination"":-0.000013, ""Periapsis"":189.764832, ""OrbitalPeriod"":149833.687500, ""RotationPeriod"":151701.406250, ""AxialTilt"":-0.503398 }";
        private const string DetailedPlanetSample = @"{ ""timestamp"":""2018-12-12T23:26:35Z"", ""event"":""Scan"", ""ScanType"":""Detailed"", ""BodyName"":""HIP 32328 8 a"", ""BodyID"":67, ""Parents"":[ {""Planet"":65}, {""Null"":64}, {""Null"":57}, {""Star"":0} ], ""DistanceFromArrivalLS"":4570.768066, ""TidalLock"":true, ""TerraformState"":"""", ""PlanetClass"":""Rocky body"", ""Atmosphere"":"""", ""AtmosphereType"":""None"", ""Volcanism"":""metallic magma volcanism"", ""MassEM"":0.002038, ""Radius"":890174.875000, ""SurfaceGravity"":1.025040, ""SurfaceTemperature"":127.420219, ""SurfacePressure"":0.000000, ""Landable"":true, ""Materials"":[ { ""Name"":""iron"", ""Percent"":19.139250 }, { ""Name"":""sulphur"", ""Percent"":18.473227 }, { ""Name"":""carbon"", ""Percent"":15.534070 }, { ""Name"":""nickel"", ""Percent"":14.476127 }, { ""Name"":""phosphorus"", ""Percent"":9.945179 }, { ""Name"":""chromium"", ""Percent"":8.607556 }, { ""Name"":""manganese"", ""Percent"":7.904315 }, { ""Name"":""arsenic"", ""Percent"":2.443018 }, { ""Name"":""cadmium"", ""Percent"":1.486249 }, { ""Name"":""antimony"", ""Percent"":1.155232 }, { ""Name"":""mercury"", ""Percent"":0.835779 } ], ""Composition"":{ ""Ice"":0.000000, ""Rock"":0.908934, ""Metal"":0.091066 }, ""SemiMajorAxis"":186318624.000000, ""Eccentricity"":0.000940, ""OrbitalInclination"":-0.000783, ""Periapsis"":78.473717, ""OrbitalPeriod"":89320.718750, ""RotationPeriod"":89322.804688, ""AxialTilt"":-0.162387 }";

        private const string AutoRingSample = @"{ ""timestamp"":""2018-12-23T00:26:36Z"", ""event"":""Scan"", ""ScanType"":""AutoScan"", ""BodyName"":""HIP 32071 A Belt Cluster 4"", ""BodyID"":5, ""Parents"":[ {""Ring"":1}, {""Star"":0} ], ""DistanceFromArrivalLS"":246.925949 }";
        private const string DetailedRingSample = @"{ ""timestamp"":""2018-12-12T23:25:30Z"", ""event"":""Scan"", ""ScanType"":""Detailed"", ""BodyName"":""HIP 32328 A Belt Cluster 5"", ""BodyID"":9, ""Parents"":[ {""Ring"":4}, {""Star"":0} ], ""DistanceFromArrivalLS"":744.149048 }";

        [Fact]
        public void AutoStar()
        {
            var autoStar = JsonConvert.DeserializeObject<Scan>(AutoStarSample);

            Assert.Equal("Core Sys Sector ON-T b3-5 A", autoStar.BodyName);
            Assert.Equal(2, autoStar.BodyID);

            Assert.Equal(2, autoStar.Parents.Count);
            // TODO: cannot currently test deserialization that uses a custom converter
            // that only exists in a higher-level project

            Assert.Equal(0.359375m, autoStar.StellarMass);

            Assert.Null(autoStar.Rings);

            Assert.Null(autoStar.AtmosphereComposition);

            Assert.Null(autoStar.Composition);
        }

        [Fact]
        public void DetailedStar()
        {
            var detailedStar = JsonConvert.DeserializeObject<Scan>(DetailedStarSample);

            Assert.Equal("Kipsigines B", detailedStar.BodyName);
            Assert.Equal(2, detailedStar.BodyID);

            Assert.Single(detailedStar.Parents);
            // TODO: cannot currently test deserialization that uses a custom converter
            // that only exists in a higher-level project

            Assert.Equal(0.125000m, detailedStar.StellarMass);

            Assert.Single(detailedStar.Rings);
            Assert.Equal(2.1301e+12m, detailedStar.Rings[0].MassMT);
            Assert.Equal("Kipsigines B A Belt", detailedStar.Rings[0].Name);

            Assert.Null(detailedStar.AtmosphereComposition);

            Assert.Null(detailedStar.Composition);
        }

        [Fact]
        public void AutoPlanet()
        {
            var autoPlanet = JsonConvert.DeserializeObject<Scan>(AutoPlanetSample);

            Assert.Equal("HIP 32328 1", autoPlanet.BodyName);
            Assert.Equal(1, autoPlanet.BodyID);
            Assert.Equal("Metal rich body", autoPlanet.PlanetClass);

            Assert.Single(autoPlanet.Parents);
            // TODO: cannot currently test deserialization that uses a custom converter
            // that only exists in a higher-level project

            Assert.Null(autoPlanet.Materials);

            Assert.Single(autoPlanet.AtmosphereComposition);
            Assert.Equal("Silicates", autoPlanet.AtmosphereComposition[0].Name);
            Assert.Equal(99.999962m, autoPlanet.AtmosphereComposition[0].Percent);

            Assert.Equal(0.000000m, autoPlanet.Composition.Ice);
            Assert.Equal(0.000000m, autoPlanet.Composition.Rock);
            Assert.Equal(1.000000m, autoPlanet.Composition.Metal);
        }

        [Fact]
        public void DetailedPlanet()
        {
            var detailedPlanet = JsonConvert.DeserializeObject<Scan>(DetailedPlanetSample);

            Assert.Equal("HIP 32328 8 a", detailedPlanet.BodyName);
            Assert.Equal(67, detailedPlanet.BodyID);
            Assert.Equal("Rocky body", detailedPlanet.PlanetClass);

            Assert.Equal(4, detailedPlanet.Parents.Count);
            // TODO: cannot currently test deserialization that uses a custom converter
            // that only exists in a higher-level project

            Assert.Null(detailedPlanet.Rings);

            Assert.Equal(11, detailedPlanet.Materials.Count);
            Assert.Equal("sulphur", detailedPlanet.Materials[1].Name);
            Assert.Equal(18.473227m, detailedPlanet.Materials[1].Percent);

            Assert.Null(detailedPlanet.AtmosphereComposition);

            Assert.Equal(0.000000m, detailedPlanet.Composition.Ice);
            Assert.Equal(0.908934m, detailedPlanet.Composition.Rock);
            Assert.Equal(0.091066m, detailedPlanet.Composition.Metal);
        }

        [Fact]
        public void AutoRing()
        {
            var autoRing = JsonConvert.DeserializeObject<Scan>(AutoRingSample);

            Assert.Equal("HIP 32071 A Belt Cluster 4", autoRing.BodyName);
            Assert.Equal(5, autoRing.BodyID);

            Assert.Equal(2, autoRing.Parents.Count);
            // TODO: cannot currently test deserialization that uses a custom converter
            // that only exists in a higher-level project

            Assert.Null(autoRing.Rings);

            Assert.Null(autoRing.Materials);

            Assert.Null(autoRing.AtmosphereComposition);

            Assert.Null(autoRing.Composition);
        }

        [Fact]
        public void DetailedRing()
        {
            var detailedRing = JsonConvert.DeserializeObject<Scan>(DetailedRingSample);

            Assert.Equal("HIP 32328 A Belt Cluster 5", detailedRing.BodyName);
            Assert.Equal(9, detailedRing.BodyID);

            Assert.Equal(2, detailedRing.Parents.Count);
            // TODO: cannot currently test deserialization that uses a custom converter
            // that only exists in a higher-level project

            Assert.Null(detailedRing.Rings);

            Assert.Null(detailedRing.Materials);

            Assert.Null(detailedRing.AtmosphereComposition);

            Assert.Null(detailedRing.Composition);
        }
    }
}
