using Howatworks.PlayerJournal.Combat;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.PlayerJournal.Test
{
    public class JournalEntryTest
    {
        [Fact]
        public void DiedSolo()
        {
            const string oneKiller = @"{""timestamp"":""2016-06-10T14:32:03Z"", ""event"":""Died"", ""KillerName"":""$ShipName_Police_Independent;"", ""KillerShip"":""viper"", ""KillerRank"":""Deadly"" }";
            var died = JsonConvert.DeserializeObject<Died>(oneKiller);
            Assert.Equal("$ShipName_Police_Independent;", died.KillerName);
            Assert.Null(died.Killers);
        }

        [Fact]
        public void DiedWing()
        {
            const string manyKillers = @"{ ""timestamp"":""2016-06-10T14:32:03Z"", ""event"":""Died"", ""Killers"":[ { ""Name"":""Cmdr HRC1"", ""Ship"":""Vulture"", ""Rank"":""Competent"" }, { ""Name"":""Cmdr HRC2"", ""Ship"":""Python"", ""Rank"":""Master"" } ] }";
            var died = JsonConvert.DeserializeObject<Died>(manyKillers);
            Assert.Null(died.KillerName);
            Assert.Equal("Cmdr HRC1", died.Killers[0].Name);
            Assert.Equal("Cmdr HRC2", died.Killers[1].Name);

        }
    }
}
