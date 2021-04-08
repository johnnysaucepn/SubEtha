using Newtonsoft.Json;

namespace Howatworks.Assistant.Core.Messages
{
    public class ControlStateMessage : IControlState, IAssistantMessage
    {
        public AssistantMessageType MessageType => AssistantMessageType.ControlState;

        [JsonProperty]
        public bool LandingGearDown { get; set; }

        [JsonProperty]
        public bool Supercruise { get; set; }

        [JsonProperty]
        public bool Srv { get; set; }

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

        [JsonProperty]
        public bool FssMode { get; set; }

        [JsonProperty]
        public bool SaaMode { get; set; }
    }
}