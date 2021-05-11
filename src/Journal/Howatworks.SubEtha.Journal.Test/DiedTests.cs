using Howatworks.SubEtha.Journal.Combat;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class DiedTests
    {
        private const string OneKillerSample = @"{""timestamp"":""2016-06-10T14:32:03Z"", ""event"":""Died"", ""KillerName"":""$ShipName_Police_Independent;"", ""KillerShip"":""viper"", ""KillerRank"":""Deadly"" }";
        private const string ManyKillersSample = @"{ ""timestamp"":""2016-06-10T14:32:03Z"", ""event"":""Died"", ""Killers"":[ { ""Name"":""Cmdr HRC1"", ""Ship"":""Vulture"", ""Rank"":""Competent"" }, { ""Name"":""Cmdr HRC2"", ""Ship"":""Python"", ""Rank"":""Master"" } ] }";

        [Fact]
        public void DiedSolo()
        {
            var died = JsonConvert.DeserializeObject<Died>(OneKillerSample);
            Assert.Equal("$ShipName_Police_Independent;", died.KillerName);
            Assert.Null(died.Killers);
        }

        [Fact]
        public void DiedWing()
        {
            var died = JsonConvert.DeserializeObject<Died>(ManyKillersSample);
            Assert.Null(died.KillerName);
            Assert.Equal("Cmdr HRC1", died.Killers[0].Name);
            Assert.Equal("Cmdr HRC2", died.Killers[1].Name);
        }
    }
}
