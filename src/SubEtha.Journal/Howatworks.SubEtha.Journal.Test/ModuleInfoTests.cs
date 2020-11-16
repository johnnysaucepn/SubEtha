using Howatworks.SubEtha.Journal.Other;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class ModuleInfoTests
    {
        /// <summary>
        /// Truncated sample.
        /// </summary>
        private const string Sample =
            @"{ ""timestamp"":""2019-12-16T01:57:31Z"", ""event"":""ModuleInfo"", ""Modules"":[ "
            + @"{ ""Slot"":""MainEngines"", ""Item"":""int_engine_size7_class2"", ""Power"":6.840000, ""Priority"":0 }, "
            + @"{ ""Slot"":""MediumHardpoint2"", ""Item"":""hpt_pulselaserburst_turret_medium"", ""Power"":0.980000, ""Priority"":0 }, "
            + @"{ ""Slot"":""PowerDistributor"", ""Item"":""int_powerdistributor_size5_class5"", ""Power"":0.740000, ""Priority"":0 }, "
            + @"{ ""Slot"":""LifeSupport"", ""Item"":""int_lifesupport_size5_class2"", ""Power"":0.640000, ""Priority"":0 }, "
            + @"{ ""Slot"":""CargoHatch"", ""Item"":""modularcargobaydoor"", ""Power"":0.600000, ""Priority"":2 }, "
            + @"{ ""Slot"":""FrameShiftDrive"", ""Item"":""int_hyperdrive_size6_class3"", ""Power"":0.560000, ""Priority"":0 }, "
            + @"{ ""Slot"":""WeaponColour"", ""Item"":""weaponcustomisation_red"", ""Power"":0.000000 }, "
            + @"{ ""Slot"":""EngineColour"", ""Item"":""enginecustomisation_red"", ""Power"":0.000000 }, "
            + @"{ ""Slot"":""DataLinkScanner"", ""Item"":""hpt_shipdatalinkscanner"", ""Power"":0.000000, ""Priority"":0 }, "
            + @"{ ""Slot"":""CodexScanner"", ""Item"":""int_codexscanner"", ""Power"":0.000000 }, "
            + @"{ ""Slot"":""DiscoveryScanner"", ""Item"":""int_stellarbodydiscoveryscanner_standard"", ""Power"":0.000000 } "
            + " ] }";

        [Fact]
        public void Modules()
        {
            var moduleInfo = JsonConvert.DeserializeObject<ModuleInfo>(Sample);

            Assert.Equal(11, moduleInfo.Modules.Count);
            Assert.Equal("CargoHatch", moduleInfo.Modules[4].Slot);
            Assert.Equal("modularcargobaydoor", moduleInfo.Modules[4].Item);
            Assert.Equal(0.600000m, moduleInfo.Modules[4].Power);
            Assert.Equal(2, moduleInfo.Modules[4].Priority);
        }
    }
}
