using Howatworks.SubEtha.Journal.Combat;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class BountyTests
    {
        private const string ShipSample = @"{
            ""timestamp"": ""2018-04-17T11:11:02Z"",
            ""event"": ""Bounty"",
            ""Rewards"": [
                {
                    ""Faction"": ""Nehet Patron's Principles"",
                    ""Reward"": 5620
                }
            ],
            ""Target"": ""empire_eagle"",
            ""TotalReward"": 5620,
            ""VictimFaction"": ""Nehet Progressive Party""
            }";

        private const string SkimmerSample = @"{
            ""timestamp"": ""2018-05-20T21:19:58Z"",
            ""event"": ""Bounty"",
            ""Faction"": ""HIP 18828 Empire Consulate"",
            ""Target"": ""Skimmer"",
            ""Reward"": 1000,
            ""VictimFaction"": ""HIP 18828 Empire Consulate""
            }";

        [Fact]
        public void Ship()
        {
            var ship = JsonConvert.DeserializeObject<Bounty>(ShipSample);

            Assert.Single(ship.Rewards);
            Assert.Equal("Nehet Patron's Principles", ship.Rewards[0].Faction);
            Assert.Equal(5620, ship.Rewards[0].Reward);
            Assert.Equal("empire_eagle", ship.Target);
            Assert.Equal("Nehet Progressive Party", ship.VictimFaction);
            Assert.Equal(5620, ship.TotalReward);
            Assert.Null(ship.SharedWithOthers);
            Assert.Null(ship.Faction);
            Assert.Null(ship.Reward);
        }

        [Fact]
        public void Skimmer()
        {
            var skimmer = JsonConvert.DeserializeObject<Bounty>(SkimmerSample);

            Assert.Null(skimmer.Rewards);
            Assert.Equal("Skimmer", skimmer.Target);
            Assert.Equal("HIP 18828 Empire Consulate", skimmer.VictimFaction);
            Assert.Null(skimmer.TotalReward);
            Assert.Null(skimmer.SharedWithOthers);
            Assert.Equal("HIP 18828 Empire Consulate", skimmer.Faction);
            Assert.Equal(1000, skimmer.Reward);
        }
    }
}
