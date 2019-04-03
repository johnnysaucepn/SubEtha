namespace Thumb.Plugin.Controller
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
    }
}