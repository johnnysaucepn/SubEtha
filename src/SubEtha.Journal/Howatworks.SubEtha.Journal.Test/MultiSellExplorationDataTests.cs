using Howatworks.SubEtha.Journal.Exploration;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class MultiSellExplorationDataTests
    {
        private static string Sample = @"{ ""timestamp"":""2019-12-16T02:47:15Z"", ""event"":""MultiSellExplorationData"", ""Discovered"":[ { ""SystemName"":""Kipsigines"", ""NumBodies"":12 }, { ""SystemName"":"""", ""NumBodies"":3 }, { ""SystemName"":""LHS 3003"", ""NumBodies"":1 } ], ""BaseValue"":15811, ""Bonus"":0, ""TotalEarnings"":15811 }";

        [Fact]
        public void DiscoveredItems()
        {
            var sell = JsonConvert.DeserializeObject<MultiSellExplorationData>(Sample);

            Assert.Equal(3, sell.Discovered.Count);
            Assert.Equal("LHS 3003", sell.Discovered[2].SystemName);
            Assert.Equal(1, sell.Discovered[2].NumBodies);
            Assert.Equal(15811, sell.TotalEarnings);
        }
    }
}
