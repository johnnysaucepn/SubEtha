using Howatworks.SubEtha.Journal.FleetCarriers;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class CarrierShipPackTests
    {
        private const string CostSample = @"{
            ""timestamp"": ""2020-03-16T09:25:39Z"",
            ""event"": ""CarrierShipPack"",
            ""CarrierID"": 3700005632,
            ""Operation"": ""BuyPack"",
            ""PackTheme"": ""Zorgon Peterson - Cargo"",
            ""PackTier"": 1,
            ""Cost"": 1668880
            }";

        private const string RefundSample = @"{
            ""timestamp"": ""2020-03-16T09:25:39Z"",
            ""event"": ""CarrierShipPack"",
            ""CarrierID"": 3700005633,
            ""Operation"": ""SellPack"",
            ""PackTheme"": ""Zorgon Peterson - Cargo"",
            ""PackTier"": 2,
            ""Refund"": 1668880
            }";

        [Fact]
        public void Cost()
        {
            var cost = JsonConvert.DeserializeObject<CarrierShipPack>(CostSample);
            Assert.Equal(3700005632, cost.CarrierID);
            Assert.Equal("BuyPack", cost.Operation);
            Assert.Equal("Zorgon Peterson - Cargo", cost.PackTheme);
            Assert.Equal(1, cost.PackTier);
            Assert.Null(cost.Refund);
            Assert.Equal(1668880, cost.Cost);
        }

        [Fact]
        public void Refund()
        {
            var refund = JsonConvert.DeserializeObject<CarrierShipPack>(RefundSample);
            Assert.Equal(3700005633, refund.CarrierID);
            Assert.Equal("SellPack", refund.Operation);
            Assert.Equal("Zorgon Peterson - Cargo", refund.PackTheme);
            Assert.Equal(2, refund.PackTier);
            Assert.Equal(1668880, refund.Refund);
            Assert.Null(refund.Cost);
        }
    }
}
