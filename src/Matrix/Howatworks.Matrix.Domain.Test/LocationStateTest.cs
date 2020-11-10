using System;
using Xunit;

namespace Howatworks.Matrix.Domain.Test
{
    public class LocationStateTest
    {
        private static DateTimeOffset Before => DateTimeOffset.Parse("2020-10-26 09:00");
        private static DateTimeOffset After => DateTimeOffset.Parse("2020-10-26 10:00");

        private static Body HomeMoon => new Body { Name = "Home", Type = "Moon" };
        private static Body HomeStation => new Body { Name = "Home", Type = "Station" };
        
        private static StarSystem Lave => new StarSystem { Name = "Lave", Coords = new [] { 1m, 2m, 3m } };
        private static StarSystem NotQuiteLave => new StarSystem { Name = "Lave", Coords = new [] { 1m, 2m, 4m } };
        private static StarSystem LaveDisguised => new StarSystem { Name = "NotLave", Coords = new [] { 1m, 2m, 3m } };
        private static StarSystem Raxxla => new StarSystem { Name = "Raxxla" };

        private static SurfaceLocation Location1 => new SurfaceLocation { Latitude = 12.00m, Longitude = 34.00m };
        private static SurfaceLocation Location2 => new SurfaceLocation { Longitude = 12.00m, Latitude = 34.00m };
        private static SurfaceLocation LandedLocation1 => new SurfaceLocation { Latitude = 12.00m, Longitude = 34.00m, Landed = true };
        private static SurfaceLocation LandedLocation2 => new SurfaceLocation { Longitude = 12.00m, Latitude = 34.00m, Landed = true };

        private static Station StarOne => new Station { Name = "Star One", Type = "Ocellus" };
        private static Station FakeStarOne => new Station { Name = "Star One", Type = "Fake" };
        private static Station XenonBase => new Station { Name = "Xenon Base", Type = "Ocellus" };
        private static Station AnonymousStation => new Station { Type = "Ocellus" };

        private static SignalSource UnknownSignal1 => new SignalSource { Threat = null, Type = new LocalisedString("Unknown", "Unknown Signal Source") };
        private static SignalSource UnknownSignal2 => new SignalSource { Threat = null, Type = new LocalisedString("Unknown", "Unknown Signal Source?") };
        private static SignalSource LowSignal => new SignalSource { Threat = 1, Type = new LocalisedString("Unknown", "Unknown Signal Source") };
        private static SignalSource HighSignal1 => new SignalSource { Threat = 5, Type = new LocalisedString("Unknown", "Unknown Signal Source") };
        private static SignalSource HighSignal2 => new SignalSource { Threat = 5, Type = new LocalisedString("Unknown?", "Unknown Signal Source") };

        private static LocationState Default1 => new LocationState();
        private static LocationState Default2 => new LocationState();

        /// <summary>
        /// Cases where the location has NOT changed
        /// </summary>
        public class NegativeTestData : TheoryData<LocationState, LocationState>
        {
            public NegativeTestData()
            {
                Add(new LocationState(), new LocationState());

                Add(new LocationState { TimeStamp = Before }, new LocationState { TimeStamp = Before });
                Add(new LocationState { TimeStamp = Before }, new LocationState { TimeStamp = After });

                Add(new LocationState { Body = HomeMoon }, new LocationState { Body = HomeMoon });
                Add(new LocationState { Body = HomeMoon, TimeStamp = Before }, new LocationState { Body = HomeMoon, TimeStamp = After });

                Add(new LocationState { StarSystem = Lave }, new LocationState { StarSystem = Lave });
                Add(new LocationState { StarSystem = Raxxla }, new LocationState { StarSystem = Raxxla });
                Add(new LocationState { StarSystem = Lave, TimeStamp = Before }, new LocationState { StarSystem = Lave, TimeStamp = After });

                Add(new LocationState { SurfaceLocation = Location1 }, new LocationState { SurfaceLocation = Location1 });
                Add(new LocationState { SurfaceLocation = LandedLocation1, TimeStamp = Before }, new LocationState { SurfaceLocation = LandedLocation1, TimeStamp = After });

                Add(new LocationState { Station = StarOne }, new LocationState { Station = StarOne });
                Add(new LocationState { Station = AnonymousStation }, new LocationState { Station = AnonymousStation });
                Add(new LocationState { Station = StarOne, TimeStamp = Before }, new LocationState { Station = StarOne, TimeStamp = After });

                Add(new LocationState { SignalSource = UnknownSignal1 }, new LocationState { SignalSource = UnknownSignal1 });
                Add(new LocationState { SignalSource = LowSignal }, new LocationState { SignalSource = LowSignal });
                Add(new LocationState { SignalSource = HighSignal1, TimeStamp = After }, new LocationState { SignalSource = HighSignal1, TimeStamp = Before });
            }
        }

        /// <summary>
        /// Cases where the location HAS changed
        /// </summary>
        public class PositiveTestData : TheoryData<LocationState, LocationState>
        {
            public PositiveTestData()
            {
                Add(new LocationState { Body = HomeMoon }, new LocationState { Body = HomeStation });
                Add(new LocationState { Body = HomeMoon, TimeStamp = Before }, new LocationState { Body = HomeStation, TimeStamp = Before });

                Add(new LocationState { StarSystem = Lave }, new LocationState { StarSystem = NotQuiteLave });
                Add(new LocationState { StarSystem = Lave }, new LocationState { StarSystem = LaveDisguised });
                Add(new LocationState { StarSystem = NotQuiteLave }, new LocationState { StarSystem = LaveDisguised });
                Add(new LocationState { StarSystem = Lave, TimeStamp = Before }, new LocationState { StarSystem = NotQuiteLave, TimeStamp = Before });
                Add(new LocationState { StarSystem = Lave }, new LocationState { StarSystem = Raxxla });

                Add(new LocationState { SurfaceLocation = Location1 }, new LocationState { SurfaceLocation = Location2 });
                Add(new LocationState { SurfaceLocation = Location1 }, new LocationState { SurfaceLocation = LandedLocation1 });
                Add(new LocationState { SurfaceLocation = LandedLocation1 }, new LocationState { SurfaceLocation = LandedLocation2 });
                Add(new LocationState { SurfaceLocation = Location2, TimeStamp = After }, new LocationState { SurfaceLocation = LandedLocation2, TimeStamp = After });

                Add(new LocationState { Station = StarOne }, new LocationState { Station = FakeStarOne });
                Add(new LocationState { Station = StarOne }, new LocationState { Station = XenonBase });
                Add(new LocationState { Station = StarOne }, new LocationState { Station = AnonymousStation });
                Add(new LocationState { Station = FakeStarOne, TimeStamp = Before }, new LocationState { Station = AnonymousStation, TimeStamp = After });
                
                Add(new LocationState { SignalSource = UnknownSignal1 }, new LocationState { SignalSource = UnknownSignal2 });
                Add(new LocationState { SignalSource = UnknownSignal1 }, new LocationState { SignalSource = LowSignal });
                Add(new LocationState { SignalSource = UnknownSignal1 }, new LocationState { SignalSource = HighSignal1 });
                Add(new LocationState { SignalSource = UnknownSignal1 }, new LocationState { SignalSource = HighSignal2 });
                Add(new LocationState { SignalSource = LowSignal }, new LocationState { SignalSource = HighSignal1 });
                Add(new LocationState { SignalSource = HighSignal1 }, new LocationState { SignalSource = HighSignal2 });
                Add(new LocationState { SignalSource = HighSignal1, TimeStamp = After }, new LocationState { SignalSource = HighSignal2, TimeStamp = After });
            }
        }

        [Fact]
        public void HasChangedSince_Defaults_False()
        {
            // Even though objects have same properties, they are not considered the same object
            Assert.NotEqual(Default1, Default2);
            Assert.NotSame(Default1, Default2);

            // But they have not changed
            Assert.False(Default1.HasChangedSince(Default2));
        }

        [Theory]
        [ClassData(typeof(NegativeTestData))]
        public void HasChangedSince_NotChanged(LocationState before, LocationState after)
        {
            // Even though objects have same properties, they are not considered the same object
            Assert.NotEqual(before, after);
            Assert.NotSame(before, after);

            Assert.False(after.HasChangedSince(before));
        }

        [Theory]
        [ClassData(typeof(PositiveTestData))]
        public void HasChangedSince_Changed(LocationState before, LocationState after)
        {
            // Even though objects have same properties, they are not considered the same object
            Assert.NotEqual(before, after);
            Assert.NotSame(before, after);

            Assert.True(after.HasChangedSince(before));
        }

    }
}
