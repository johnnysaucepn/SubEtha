using Howatworks.SubEtha.Journal.StationServices;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class RepairTests
    {
        private const string ShipSample = @"{
            ""timestamp"": ""2016-06-10T14:32:03Z"",
            ""event"": ""Repair"",
            ""Item"": ""int_powerplant_size3_class5"",
            ""Cost"": 1100
            }";

        private const string CarrierSample = @"{
            ""timestamp"": ""2020-03-31T13:39:42Z"",
            ""event"": ""Repair"",
            ""Items"": [
                ""$hpt_dumbfiremissilerack_fixed_large_name;"",
                ""$hpt_beamlaser_gimbal_medium_name;"",
                ""$hpt_railgun_fixed_medium_name;"",
                ""$hpt_beamlaser_gimbal_medium_name;"",
                ""$hpt_dumbfiremissilerack_fixed_large_name;""
            ],
            ""Cost"": 34590
            }";

        [Fact]
        public void Ship()
        {
            var ship = JsonConvert.DeserializeObject<Repair>(ShipSample);

            Assert.Equal("int_powerplant_size3_class5", ship.Item);
            Assert.Equal(1100, ship.Cost);
            Assert.Null(ship.Items);
        }

        [Fact]
        public void Carrier()
        {
            var carrier = JsonConvert.DeserializeObject<Repair>(CarrierSample);

            Assert.Null(carrier.Item);
            Assert.Equal(5, carrier.Items.Count);
            Assert.Equal("$hpt_railgun_fixed_medium_name;", carrier.Items[2]);
            Assert.Equal(34590, carrier.Cost);
        }
    }
}
