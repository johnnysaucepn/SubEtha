using Newtonsoft.Json;

namespace Thumb.Plugin.Controller.Messages
{
    public class ControlState : IControlState
    {
        [JsonProperty]
        public bool LandingGearDown { get; set; }

        [JsonProperty]
        public bool Supercruise { get; set; }

        [JsonProperty]
        public bool HardpointsDeployed { get; set; }

        [JsonProperty]
        public bool LightsOn { get; set; }

        [JsonProperty]
        public bool CargoScoopDeployed { get; set; }

        [JsonProperty]
        public bool NightVision { get; set; }

        [JsonProperty]
        public bool HudAnalysisMode { get; set; }
    }
}