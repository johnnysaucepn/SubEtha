namespace Howatworks.Thumb.Plugin.Assistant
{
    public interface IControlState
    {
        bool LandingGearDown { get; set; }
        bool Supercruise { get; set; }
        bool HardpointsDeployed { get; set; }
        bool LightsOn { get; set; }
        bool CargoScoopDeployed { get; set; }
        bool NightVision { get; set; }
        bool HudAnalysisMode { get; set; }
        bool FssMode { get; set; }
        bool SaaMode { get; set; }
    }
}