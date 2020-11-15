using Howatworks.SubEtha.Journal.Exploration;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class SellExplorationDataTests
    {
        private static string Sample = @"{ ""timestamp"":""2016-06-10T14:32:03Z"", ""event"":""SellExplorationData"", ""Systems"":[ ""HIP 78085"", ""Praea Euq NW-W b1-3"" ], ""Discovered"":[ ""HIP 78085 A"", ""Praea Euq NW-W b1-3"", ""Praea Euq NWW b1-3 3 a"", ""Praea Euq NW-W b1-3 3"" ], ""BaseValue"":10822, ""Bonus"":3959, ""TotalEarnings"":44343 }";

        [Fact]
        public void DiscoveredItems()
        {
            var sell = JsonConvert.DeserializeObject<SellExplorationData>(Sample);

            Assert.Equal(2, sell.Systems.Count);
            Assert.Equal(4, sell.Discovered.Count);
            Assert.Equal("Praea Euq NW-W b1-3", sell.Systems[1]);
            Assert.Equal("Praea Euq NW-W b1-3 3", sell.Discovered[3]);
            Assert.Equal(3959, sell.Bonus);
        }
    }
}
