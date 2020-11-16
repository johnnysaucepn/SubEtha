using Howatworks.SubEtha.Journal.Combat;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class ShipTargetedTests
    {
        private const string Stage0Sample = @"{ ""timestamp"":""2018-12-12T23:54:54Z"", ""event"":""ShipTargeted"", ""TargetLocked"":true, ""Ship"":""diamondbackxl"", ""Ship_Localised"":""Diamondback Explorer"", ""ScanStage"":0 }";
        private const string Stage1Sample = @"{ ""timestamp"":""2018-12-23T01:28:37Z"", ""event"":""ShipTargeted"", ""TargetLocked"":true, ""Ship"":""empire_eagle"", ""Ship_Localised"":""Imperial Eagle"", ""ScanStage"":1, ""PilotName"":""$npc_name_decorate:#name=Krackenhead;"", ""PilotName_Localised"":""Krackenhead"", ""PilotRank"":""Master"" }";
        private const string Stage2Sample = @"{ ""timestamp"":""2019-01-06T22:52:24Z"", ""event"":""ShipTargeted"", ""TargetLocked"":true, ""Ship"":""belugaliner"", ""Ship_Localised"":""Beluga Liner"", ""ScanStage"":2, ""PilotName"":""$npc_name_decorate:#name=Beautiful Corner Tours;"", ""PilotName_Localised"":""Beautiful Corner Tours"", ""PilotRank"":""Deadly"", ""ShieldHealth"":100.000000, ""HullHealth"":100.000000 }";
        private const string Stage3Sample = @"{ ""timestamp"":""2019-01-07T01:19:21Z"", ""event"":""ShipTargeted"", ""TargetLocked"":true, ""Ship"":""type9"", ""Ship_Localised"":""Type-9 Heavy"", ""ScanStage"":3, ""PilotName"":""$npc_name_decorate:#name=Paul Anderson;"", ""PilotName_Localised"":""Paul Anderson"", ""PilotRank"":""Dangerous"", ""ShieldHealth"":100.000000, ""HullHealth"":100.000000, ""Faction"":""Oddyssey Explorers"", ""LegalStatus"":""Clean"" }";

        [Fact]
        public void TargetLocked()
        {
            var stage0 = JsonConvert.DeserializeObject<ShipTargeted>(Stage0Sample);

            Assert.True(stage0.TargetLocked);
            Assert.Equal(0, stage0.ScanStage);
            Assert.Null(stage0.PilotName_Localised);
            Assert.Null(stage0.ShieldHealth);
            Assert.Null(stage0.LegalStatus);
        }

        [Fact]
        public void Stage1Scan()
        {
            var stage1 = JsonConvert.DeserializeObject<ShipTargeted>(Stage1Sample);

            Assert.True(stage1.TargetLocked);
            Assert.Equal(1, stage1.ScanStage);
            Assert.Equal("Krackenhead", stage1.PilotName_Localised);
            Assert.Null(stage1.ShieldHealth);
            Assert.Null(stage1.LegalStatus);
        }

        [Fact]
        public void Stage2Scan()
        {
            var stage2 = JsonConvert.DeserializeObject<ShipTargeted>(Stage2Sample);

            Assert.True(stage2.TargetLocked);
            Assert.Equal(2, stage2.ScanStage);
            Assert.Equal("Beautiful Corner Tours", stage2.PilotName_Localised);
            Assert.Equal(100, stage2.ShieldHealth);
            Assert.Null(stage2.LegalStatus);
        }

        [Fact]
        public void Stage3Scan()
        {
            var stage3 = JsonConvert.DeserializeObject<ShipTargeted>(Stage3Sample);

            Assert.True(stage3.TargetLocked);
            Assert.Equal(3, stage3.ScanStage);
            Assert.Equal("Paul Anderson", stage3.PilotName_Localised);
            Assert.Equal(100, stage3.ShieldHealth);
            Assert.Equal("Clean", stage3.LegalStatus);
        }
    }
}
