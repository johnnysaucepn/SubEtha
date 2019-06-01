using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Howatworks.SubEtha.Bindings
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BindingSet
    {

        [XmlAttribute]
        public string PresetName { get; set; }
        [XmlAttribute]
        public int MajorVersion { get; set; }
        [XmlAttribute]
        public int MinorVersion { get; set; }
        [XmlAttribute]
        public int SortOrder { get; set; }

        public string KeyboardLayout { get; set; }

        public Axis YawAxisRaw { get; set; }
        public Button YawLeftButton { get; set; }
        public Button YawRightButton { get; set; }
        public Setting<string> YawToRollMode { get; set; } // TODO: enum?
        public Setting<decimal> YawToRollSensitivity { get; set; }
        public Button YawToRollButton { get; set; }

        public Axis RollAxisRaw { get; set; }
        public Button RollLeftButton { get; set; }
        public Button RollRightButton { get; set; }

        public Axis PitchAxisRaw { get; set; }
        public Button PitchUpButton { get; set; }
        public Button PitchDownButton { get; set; }

        public Setting<string> MouseXMode { get; set; } // TODO: check data type
        public Setting<string> MouseYMode { get; set; } // TODO: check data type
        public Setting<decimal> MouseXDecay { get; set; } // TODO: check data type
        public Setting<decimal> MouseYDecay { get; set; } // TODO: check data type
        public Button MouseReset { get; set; }
        public Setting<decimal> MouseDecayRate { get; set; }
        public Setting<int> MouseGUI { get; set; } // TODO: check data type
        public Setting<bool> MouseHeadlook { get; set; }
        public Setting<bool> MouseHeadlookInvert { get; set; }
        public Setting<decimal> MouseHeadlookSensitivity { get; set; }
        public Setting<decimal> MouseSensitivity { get; set; }
        public Setting<decimal> MouseDeadZone { get; set; }
        public Setting<decimal> MouseLinearity { get; set; }

        public Axis LateralThrustRaw { get; set; }
        public Button LeftThrustButton { get; set; }
        public Button RightThrustButton { get; set; }
        public Axis VerticalThrustRaw { get; set; }
        public Button UpThrustButton { get; set; }
        public Button DownThrustButton { get; set; }
        public Axis AheadThrust { get; set; }
        public Button ForwardThrustButton { get; set; }
        public Button BackwardThrustButton { get; set; }

        public Axis YawAxisAlternate { get; set; }
        public Axis RollAxisAlternate { get; set; }
        public Axis PitchAxisAlternate { get; set; }
        public Axis LateralThrustAlternate { get; set; }
        public Axis VerticalThrustAlternate { get; set; }

        public Button UseAlternateFlightValuesToggle { get; set; }

        public Setting<string> ThrottleRange { get; set; } // TODO: check data type
        public Axis ThrottleAxis { get; set; }
        public Button ToggleReverseThrottleInput { get; set; }
        public Setting<decimal> ThrottleIncrement { get; set; }
        public Button ForwardKey { get; set; }
        public Button BackwardKey { get; set; }
        public Button SetSpeedMinus100 { get; set; }
        public Button SetSpeedMinus75 { get; set; }
        public Button SetSpeedMinus50 { get; set; }
        public Button SetSpeedMinus25 { get; set; }
        public Button SetSpeedZero { get; set; }
        public Button SetSpeed25 { get; set; }
        public Button SetSpeed50 { get; set; }
        public Button SetSpeed75 { get; set; }
        public Button SetSpeed100 { get; set; }

        public Axis YawAxis_Landing { get; set; }
        public Button YawLeftButton_Landing { get; set; }
        public Button YawRightButton_Landing { get; set; }
        public Setting<string> YawToRollMode_Landing { get; set; } // TODO: enum?

        public Axis PitchAxis_Landing { get; set; }
        public Button PitchUpButton_Landing { get; set; }
        public Button PitchDownButton_Landing { get; set; }

        public Axis RollAxis_Landing { get; set; }
        public Button RollLeftButton_Landing { get; set; }
        public Button RollRightButton_Landing { get; set; }

        public Axis LateralThrust_Landing { get; set; }
        public Button LeftThrustButton_Landing { get; set; }
        public Button RightThrustButton_Landing { get; set; }
        public Axis VerticalThrust_Landing { get; set; }
        public Button UpThrustButton_Landing { get; set; }
        public Button DownThrustButton_Landing { get; set; }
        public Axis AheadThrust_Landing { get; set; }
        public Button ForwardThrustButton_Landing { get; set; }
        public Button BackwardThrustButton_Landing { get; set; }

        public Button ToggleFlightAssist { get; set; }
        public Setting<string> YawToRollMode_FAOff { get; set; } // TODO: check data type
        public Button UseBoostJuice { get; set; }

        public Button SelectTarget { get; set; }
        public Button CycleNextTarget { get; set; }
        public Button CyclePreviousTarget { get; set; }
        public Button SelectHighestThreat { get; set; }
        public Button CycleNextHostileTarget { get; set; }
        public Button CyclePreviousHostileTarget { get; set; }

        public Button TargetWingman0 { get; set; }
        public Button TargetWingman1 { get; set; }
        public Button TargetWingman2 { get; set; }
        public Button SelectTargetsTarget { get; set; }
        public Button WingNavLock { get; set; }

        public Button TargetNextRouteSystem { get; set; }

        public Button CycleNextSubsystem { get; set; }
        public Button CyclePreviousSubsystem { get; set; }

        public Setting<string> GunsightSystem { get; set; } // TODO: enum? Bindings_TraditionalGunsights

        public Button PrimaryFire { get; set; }
        public Button SecondaryFire { get; set; }
        public Button CycleFireGroupNext { get; set; }
        public Button CycleFireGroupPrevious { get; set; }
        public Button DeployHardpointToggle { get; set; }
        public Setting<bool> DeployHardpointsOnFire { get; set; }

        public Button ToggleButtonUpInput { get; set; }
        public Button DeployHeatSink { get; set; }
        public Button ShipSpotLightToggle { get; set; }
        public Button OrbitLinesToggle { get; set; }

        public Axis RadarRangeAxis { get; set; }
        public Button RadarIncreaseRange { get; set; }
        public Button RadarDecreaseRange { get; set; }

        public Button IncreaseEnginesPower { get; set; }
        public Button IncreaseWeaponsPower { get; set; }
        public Button IncreaseSystemsPower { get; set; }
        public Button ResetPowerDistribution { get; set; }

        public Button HMDReset { get; set; }

        public Button ToggleCargoScoop { get; set; }
        public Button EjectAllCargo { get; set; }
        public Button LandingGearToggle { get; set; }

        public Button MicrophoneMute { get; set; }

        public Button UseShieldCell { get; set; }
        public Button FireChaffLauncher { get; set; }

        public Button PhotoCameraToggle { get; set; }
        public Button PhotoCameraToggle_Buggy { get; set; }

        public Button HyperSuperCombination { get; set; }
        public Button Supercruise { get; set; }
        public Button Hyperspace { get; set; }
        public Button DisableRotationCorrectToggle { get; set; }

        public Button GalaxyMapOpen { get; set; }
        public Button SystemMapOpen { get; set; }
        public Button ShowPGScoreSummaryInput { get; set; }

        public Setting<string> UIFocusMode { get; set; } // TODO: enum? Bindings_FocusModeHold
        public Button UIFocus { get; set; }
        public Button Pause { get; set; }

        // Note: inconsistencies of spelling 'HeadLook' vs 'Headlook'
        public Button HeadLookToggle { get; set; }
        public Setting<bool> HeadlookDefault { get; set; }
        public Setting<bool> HeadlookResetOnToggle { get; set; }
        public Setting<decimal> HeadlookSensitivity { get; set; }
        public Setting<bool> MotionHeadlook { get; set; }

        public Button FocusLeftPanel { get; set; }
        public Button FocusRightPanel { get; set; }
        public Button FocusRadarPanel { get; set; }
        public Button FocusCommsPanel { get; set; }
        public Button QuickCommsPanel { get; set; }
        public Setting<bool> FocusOnTextEntryField { get; set; } // TODO: check data type

        public Setting<string> MuteButtonMode { get; set; } // TODO: enum? mute_toggle
        public Setting<string> CqCMuteButtonMode { get; set; } // TODO: enum? mute_pushToTalk

        public Setting<bool> EnableCameraLockOn { get; set; } // TODO: check data type

        public Setting<string> LeftPanelFocusOptions { get; set; } // TODO: check data type
        public Setting<string> CommsPanelFocusOptions { get; set; } // TODO: check data type
        public Setting<string> RolePanelFocusOptions { get; set; } // TODO: check data type
        public Setting<string> RightPanelFocusOptions { get; set; } // TODO: check data type

        public Button UI_Up { get; set; }
        public Button UI_Down { get; set; }
        public Button UI_Left { get; set; }
        public Button UI_Right { get; set; }
        public Button UI_Select { get; set; }
        public Button UI_Back { get; set; }
        public Button UI_Toggle { get; set; }
        public Button CycleNextPanel { get; set; }
        public Button CyclePreviousPanel { get; set; }
        public Button CycleNextPage { get; set; }
        public Button CyclePreviousPage { get; set; }

        //public Setting<bool> HeadlookDefault { get; set; } // TODO: duplicate?
        public Setting<decimal> HeadlookIncrement { get; set; }
        public Setting<string> HeadlookMode { get; set; } // TODO: enum?
        //public Setting<bool> HeadlookResetOnToggle { get; set; } // TODO: duplicate?
        //public Setting<decimal> HeadlookSensitivity { get; set; } // TODO: duplicate?
        public Button HeadLookReset { get; set; }
        public Button HeadLookPitchUp { get; set; }
        public Button HeadLookPitchDown { get; set; }
        public Axis HeadLookPitchAxisRaw { get; set; }
        public Button HeadLookYawLeft { get; set; }
        public Button HeadLookYawRight { get; set; }
        public Axis HeadLookYawAxis { get; set; }

        public Axis CamPitchAxis { get; set; }
        public Button CamPitchUp { get; set; }
        public Button CamPitchDown { get; set; }
        public Button CamYawAxis { get; set; }
        public Button CamYawLeft { get; set; }
        public Button CamYawRight { get; set; }
        public Axis CamTranslateYAxis { get; set; }
        public Button CamTranslateForward { get; set; }
        public Button CamTranslateBackward { get; set; }
        public Axis CamTranslateXAxis { get; set; }
        public Button CamTranslateLeft { get; set; }
        public Button CamTranslateRight { get; set; }
        public Axis CamTranslateZAxis { get; set; }
        public Button CamTranslateUp { get; set; }
        public Button CamTranslateDown { get; set; }
        public Axis CamZoomAxis { get; set; }
        public Button CamZoomIn { get; set; }
        public Button CamZoomOut { get; set; }
        public Button CamTranslateZHold { get; set; }

        public Button GalaxyMapHome { get; set; }

        public Button ToggleDriveAssist { get; set; }
        public Setting<bool> DriveAssistDefault { get; set; }


        public Button ChargeECM { get; set; }

        public Setting<bool> EnableRumbleTrigger { get; set; } // TODO: check data type
        public Setting<bool> EnableMenuGroups { get; set; } // TODO: check data type

        public Button WeaponColourToggle { get; set; }
        public Button EngineColourToggle { get; set; }
        public Button NightVisionToggle { get; set; }


















        public Button FriendsMenu { get; set; }
        public Button OpenCodexGoToDiscovery { get; set; }
        public Button PlayerHUDModeToggle { get; set; }




        public Setting<bool> HeadlookSmoothing { get; set; }



    }
}
