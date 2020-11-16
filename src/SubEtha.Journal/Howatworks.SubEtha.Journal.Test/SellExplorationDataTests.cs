using Howatworks.SubEtha.Journal.Exploration;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class SellExplorationDataTests
    {
        private const string Sample = @"{ ""timestamp"":""2016-06-10T14:32:03Z"", ""event"":""SellExplorationData"", ""Systems"":[ ""HIP 78085"", ""Praea Euq NW-W b1-3"" ], ""Discovered"":[ ""HIP 78085 A"", ""Praea Euq NW-W b1-3"", ""Praea Euq NWW b1-3 3 a"", ""Praea Euq NW-W b1-3 3"" ], ""BaseValue"":10822, ""Bonus"":3959, ""TotalEarnings"":44343 }";

        [Fact]
        public void Properties()
        {
            var sell = JsonConvert.DeserializeObject<SellExplorationData>(Sample);

            Assert.Equal(3959, sell.Bonus);
        }

        [Fact]
        public void Discovered()
        {
            var sell = JsonConvert.DeserializeObject<SellExplorationData>(Sample);

            Assert.Equal(4, sell.Discovered.Count);
            Assert.Equal("Praea Euq NW-W b1-3 3", sell.Discovered[3]);
        }

        [Fact]
        public void Systems()
        {
            var sell = JsonConvert.DeserializeObject<SellExplorationData>(Sample);

            Assert.Equal(2, sell.Systems.Count);
            Assert.Equal("Praea Euq NW-W b1-3", sell.Systems[1]);
        }

    }
}
