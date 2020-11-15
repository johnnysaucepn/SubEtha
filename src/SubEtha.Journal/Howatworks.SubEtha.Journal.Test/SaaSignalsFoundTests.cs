using Howatworks.SubEtha.Journal.Exploration;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class SaaSignalsFoundTests
    {
        private static string Sample = @"{ ""timestamp"":""2019-04-17T13:38:18Z"", ""event"":""SAASignalsFound"", ""BodyName"":""Hermitage 4 A Ring"", ""SystemAddress"":5363877956440, ""BodyID"":11, ""Signals"":[ { ""Type"":""LowTemperatureDiamond"", ""Type_Localised"":""Low Temperature Diamonds"", ""Count"":1 }, { ""Type"":""Alexandrite"", ""Count"":1 } ] }";

        [Fact]
        public void Signals()
        {
            var signals = JsonConvert.DeserializeObject<SaaSignalsFound>(Sample);

            Assert.Equal("Hermitage 4 A Ring", signals.BodyName);
            Assert.Equal(2, signals.Signals.Count);

            Assert.Equal("LowTemperatureDiamond", signals.Signals[0].Type);
            Assert.Equal("Low Temperature Diamonds", signals.Signals[0].Type_Localised);
            Assert.Equal(1, signals.Signals[0].Count);

            Assert.Equal("Alexandrite", signals.Signals[1].Type);
            Assert.Null(signals.Signals[1].Type_Localised);
            Assert.Equal(1, signals.Signals[1].Count);
        }
    }
}
