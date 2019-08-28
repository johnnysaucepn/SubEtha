namespace Howatworks.SubEtha.Bindings
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class BindingSet
    {
       public Setting<string> MouseXMode { get; set; } // TODO: enum?
       public Setting<bool> MouseXDecay { get; set; } // TODO: check data type
       public Setting<string> MouseYMode { get; set; } // TODO: enum?
       public Setting<bool> MouseYDecay { get; set; } // TODO: check data type
       public Button MouseReset { get; set; }
       public Setting<decimal> MouseSensitivity { get; set; }
       public Setting<decimal> MouseDecayRate { get; set; }
       public Setting<decimal> MouseDeadzone { get; set; }
       public Setting<decimal> MouseLinearity { get; set; }
       public Setting<bool> MouseGUI { get; set; } // TODO: check data type
       public Axis YawAxisRaw { get; set; }
       public Button YawLeftButton { get; set; }
       public Button YawRightButton { get; set; }
       public Setting<string> YawToRollMode { get; set; } // TODO: enum?
       public Setting<decimal> YawToRollSensitivity { get; set; }
       public Setting<string> YawToRollMode_FAOff { get; set; } // TODO: enum?
       public Button YawToRollButton { get; set; }
       public Axis RollAxisRaw { get; set; }
       public Button RollLeftButton { get; set; }
       public Button RollRightButton { get; set; }
       public Axis PitchAxisRaw { get; set; }
       public Button PitchUpButton { get; set; }
       public Button PitchDownButton { get; set; }
       public Axis LateralThrustRaw { get; set; }
       public Button LeftThrustButton { get; set; }
       public Button RightThrustButton { get; set; }
       public Axis VerticalThrustRaw { get; set; }
       public Button UpThrustButton { get; set; }
       public Button DownThrustButton { get; set; }
       public Axis AheadThrust { get; set; }
       public Button ForwardThrustButton { get; set; }
       public Button BackwardThrustButton { get; set; }
       public Button UseAlternateFlightValuesToggle { get; set; }
       public Axis YawAxisAlternate { get; set; }
       public Axis RollAxisAlternate { get; set; }
       public Axis PitchAxisAlternate { get; set; }
       public Axis LateralThrustAlternate { get; set; }
       public Axis VerticalThrustAlternate { get; set; }
       public Axis ThrottleAxis { get; set; }
       public Setting<string> ThrottleRange { get; set; } // TODO: enum?
       public Button ToggleReverseThrottleInput { get; set; }
       public Button ForwardKey { get; set; }
       public Button BackwardKey { get; set; }
       public Setting<decimal> ThrottleIncrement { get; set; }
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
       public Button UseBoostJuice { get; set; }
       public Button HyperSuperCombination { get; set; }
       public Button Supercruise { get; set; }
       public Button Hyperspace { get; set; }
       public Button DisableRotationCorrectToggle { get; set; }
       public Button OrbitLinesToggle { get; set; }
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
       public Button CycleNextSubsystem { get; set; }
       public Button CyclePreviousSubsystem { get; set; }
       public Button TargetNextRouteSystem { get; set; }
       public Button PrimaryFire { get; set; }
       public Button SecondaryFire { get; set; }
       public Button CycleFireGroupNext { get; set; }
       public Button CycleFireGroupPrevious { get; set; }
       public Button DeployHardpointToggle { get; set; }
       public Setting<bool> DeployHardpointsOnFire { get; set; } // TODO: check data type
       public Button ToggleButtonUpInput { get; set; }
       public Button DeployHeatSink { get; set; }
       public Button ShipSpotLightToggle { get; set; }
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
       public Setting<string> MuteButtonMode { get; set; } // TODO: enum?
       public Setting<string> CqcMuteButtonMode { get; set; } // TODO: enum?
       public Button UseShieldCell { get; set; }
       public Button FireChaffLauncher { get; set; }
       public Button ChargeECM { get; set; }
       public Setting<bool> EnableRumbleTrigger { get; set; } // TODO: check data type
       public Setting<bool> EnableMenuGroups { get; set; } // TODO: check data type
       public Button WeaponColourToggle { get; set; }
       public Button EngineColourToggle { get; set; }
       public Button NightVisionToggle { get; set; }
       public Button UIFocus { get; set; }
       public Setting<string> UIFocusMode { get; set; } // TODO: enum?
       public Button FocusLeftPanel { get; set; }
       public Button FocusCommsPanel { get; set; }
       public Setting<bool> FocusOnTextEntryField { get; set; } // TODO: check data type
       public Button QuickCommsPanel { get; set; }
       public Button FocusRadarPanel { get; set; }
       public Button FocusRightPanel { get; set; }
       public Setting<string> LeftPanelFocusOptions { get; set; } // TODO: enum?
       public Setting<string> CommsPanelFocusOptions { get; set; } // TODO: enum?
       public Setting<string> RolePanelFocusOptions { get; set; } // TODO: enum?
       public Setting<string> RightPanelFocusOptions { get; set; } // TODO: enum?
       public Setting<bool> EnableCameraLockOn { get; set; } // TODO: check data type
       public Button GalaxyMapOpen { get; set; }
       public Button SystemMapOpen { get; set; }
       public Button ShowPGScoreSummaryInput { get; set; }
       public Button HeadLookToggle { get; set; }
       public Button Pause { get; set; }
       public Button FriendsMenu { get; set; }
       public Button OpenCodexGoToDiscovery { get; set; }
       public Button PlayerHUDModeToggle { get; set; }
       public Button ExplorationFSSEnter { get; set; }
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
       public Setting<bool> MouseHeadlook { get; set; } // TODO: check data type
       public Setting<bool> MouseHeadlookInvert { get; set; } // TODO: check data type
       public Setting<decimal> MouseHeadlookSensitivity { get; set; }
       public Setting<bool> HeadlookDefault { get; set; } // TODO: check data type
       public Setting<decimal> HeadlookIncrement { get; set; }
       public Setting<string> HeadlookMode { get; set; } // TODO: enum?
       public Setting<bool> HeadlookResetOnToggle { get; set; } // TODO: check data type
       public Setting<decimal> HeadlookSensitivity { get; set; }
       public Setting<bool> HeadlookSmoothing { get; set; } // TODO: check data type
       public Button HeadLookReset { get; set; }
       public Button HeadLookPitchUp { get; set; }
       public Button HeadLookPitchDown { get; set; }
       public Axis HeadLookPitchAxisRaw { get; set; }
       public Button HeadLookYawLeft { get; set; }
       public Button HeadLookYawRight { get; set; }
       public Axis HeadLookYawAxis { get; set; }
       public Setting<bool> MotionHeadlook { get; set; } // TODO: check data type
       public Setting<decimal> HeadlookMotionSensitivity { get; set; }
       public Setting<bool> yawRotateHeadlook { get; set; } // TODO: check data type
       public Axis CamPitchAxis { get; set; }
       public Button CamPitchUp { get; set; }
       public Button CamPitchDown { get; set; }
       public Axis CamYawAxis { get; set; }
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
       public Setting<bool> DriveAssistDefault { get; set; } // TODO: check data type
       public Setting<string> MouseBuggySteeringXMode { get; set; } // TODO: enum?
       public Setting<bool> MouseBuggySteeringXDecay { get; set; } // TODO: check data type
       public Setting<string> MouseBuggyRollingXMode { get; set; } // TODO: enum?
       public Setting<bool> MouseBuggyRollingXDecay { get; set; } // TODO: check data type
       public Setting<string> MouseBuggyYMode { get; set; } // TODO: enum?
       public Setting<bool> MouseBuggyYDecay { get; set; } // TODO: check data type
       public Axis SteeringAxis { get; set; }
       public Button SteerLeftButton { get; set; }
       public Button SteerRightButton { get; set; }
       public Axis BuggyRollAxisRaw { get; set; }
       public Button BuggyRollLeftButton { get; set; }
       public Button BuggyRollRightButton { get; set; }
       public Axis BuggyPitchAxis { get; set; }
       public Button BuggyPitchUpButton { get; set; }
       public Button BuggyPitchDownButton { get; set; }
       public Button VerticalThrustersButton { get; set; }
       public Button BuggyPrimaryFireButton { get; set; }
       public Button BuggySecondaryFireButton { get; set; }
       public Button AutoBreakBuggyButton { get; set; }
       public Button HeadlightsBuggyButton { get; set; }
       public Button ToggleBuggyTurretButton { get; set; }
       public Button BuggyCycleFireGroupNext { get; set; }
       public Button BuggyCycleFireGroupPrevious { get; set; }
       public Button SelectTarget_Buggy { get; set; }
       public Setting<string> MouseTurretXMode { get; set; } // TODO: enum?
       public Setting<bool> MouseTurretXDecay { get; set; } // TODO: check data type
       public Setting<string> MouseTurretYMode { get; set; } // TODO: enum?
       public Setting<bool> MouseTurretYDecay { get; set; } // TODO: check data type
       public Axis BuggyTurretYawAxisRaw { get; set; }
       public Button BuggyTurretYawLeftButton { get; set; }
       public Button BuggyTurretYawRightButton { get; set; }
       public Axis BuggyTurretPitchAxisRaw { get; set; }
       public Button BuggyTurretPitchUpButton { get; set; }
       public Button BuggyTurretPitchDownButton { get; set; }
       public Setting<decimal> BuggyTurretMouseSensitivity { get; set; }
       public Setting<decimal> BuggyTurretMouseDeadzone { get; set; }
       public Setting<decimal> BuggyTurretMouseLinearity { get; set; }
       public Axis DriveSpeedAxis { get; set; }
       public Setting<string> BuggyThrottleRange { get; set; } // TODO: enum?
       public Button BuggyToggleReverseThrottleInput { get; set; }
       public Setting<decimal> BuggyThrottleIncrement { get; set; }
       public Button IncreaseSpeedButtonMax { get; set; }
       public Button DecreaseSpeedButtonMax { get; set; }
       public Axis IncreaseSpeedButtonPartial { get; set; }
       public Axis DecreaseSpeedButtonPartial { get; set; }
       public Button IncreaseEnginesPower_Buggy { get; set; }
       public Button IncreaseWeaponsPower_Buggy { get; set; }
       public Button IncreaseSystemsPower_Buggy { get; set; }
       public Button ResetPowerDistribution_Buggy { get; set; }
       public Button ToggleCargoScoop_Buggy { get; set; }
       public Button EjectAllCargo_Buggy { get; set; }
       public Button RecallDismissShip { get; set; }
       public Button UIFocus_Buggy { get; set; }
       public Button FocusLeftPanel_Buggy { get; set; }
       public Button FocusCommsPanel_Buggy { get; set; }
       public Button QuickCommsPanel_Buggy { get; set; }
       public Button FocusRadarPanel_Buggy { get; set; }
       public Button FocusRightPanel_Buggy { get; set; }
       public Button GalaxyMapOpen_Buggy { get; set; }
       public Button SystemMapOpen_Buggy { get; set; }
       public Button OpenCodexGoToDiscovery_Buggy { get; set; }
       public Button PlayerHUDModeToggle_Buggy { get; set; }
       public Button HeadLookToggle_Buggy { get; set; }
       public Button MultiCrewToggleMode { get; set; }
       public Button MultiCrewPrimaryFire { get; set; }
       public Button MultiCrewSecondaryFire { get; set; }
       public Button MultiCrewPrimaryUtilityFire { get; set; }
       public Button MultiCrewSecondaryUtilityFire { get; set; }
       public Setting<string> MultiCrewThirdPersonMouseXMode { get; set; } // TODO: enum?
       public Setting<bool> MultiCrewThirdPersonMouseXDecay { get; set; } // TODO: check data type
       public Setting<string> MultiCrewThirdPersonMouseYMode { get; set; } // TODO: enum?
       public Setting<bool> MultiCrewThirdPersonMouseYDecay { get; set; } // TODO: check data type
       public Axis MultiCrewThirdPersonYawAxisRaw { get; set; }
       public Button MultiCrewThirdPersonYawLeftButton { get; set; }
       public Button MultiCrewThirdPersonYawRightButton { get; set; }
       public Axis MultiCrewThirdPersonPitchAxisRaw { get; set; }
       public Button MultiCrewThirdPersonPitchUpButton { get; set; }
       public Button MultiCrewThirdPersonPitchDownButton { get; set; }
       public Setting<decimal> MultiCrewThirdPersonMouseSensitivity { get; set; }
       public Axis MultiCrewThirdPersonFovAxisRaw { get; set; }
       public Button MultiCrewThirdPersonFovOutButton { get; set; }
       public Button MultiCrewThirdPersonFovInButton { get; set; }
       public Button MultiCrewCockpitUICycleForward { get; set; }
       public Button MultiCrewCockpitUICycleBackward { get; set; }
       public Button OrderRequestDock { get; set; }
       public Button OrderDefensiveBehaviour { get; set; }
       public Button OrderAggressiveBehaviour { get; set; }
       public Button OrderFocusTarget { get; set; }
       public Button OrderHoldFire { get; set; }
       public Button OrderHoldPosition { get; set; }
       public Button OrderFollow { get; set; }
       public Button OpenOrders { get; set; }
       public Button PhotoCameraToggle { get; set; }
       public Button PhotoCameraToggle_Buggy { get; set; }
       public Button VanityCameraScrollLeft { get; set; }
       public Button VanityCameraScrollRight { get; set; }
       public Button ToggleFreeCam { get; set; }
       public Button VanityCameraOne { get; set; }
       public Button VanityCameraTwo { get; set; }
       public Button VanityCameraThree { get; set; }
       public Button VanityCameraFour { get; set; }
       public Button VanityCameraFive { get; set; }
       public Button VanityCameraSix { get; set; }
       public Button VanityCameraSeven { get; set; }
       public Button VanityCameraEight { get; set; }
       public Button VanityCameraNine { get; set; }
       public Button FreeCamToggleHUD { get; set; }
       public Button FreeCamSpeedInc { get; set; }
       public Button FreeCamSpeedDec { get; set; }
       public Axis MoveFreeCamY { get; set; }
       public Setting<string> ThrottleRangeFreeCam { get; set; } // TODO: enum?
       public Button ToggleReverseThrottleInputFreeCam { get; set; }
       public Button MoveFreeCamForward { get; set; }
       public Button MoveFreeCamBackwards { get; set; }
       public Axis MoveFreeCamX { get; set; }
       public Button MoveFreeCamRight { get; set; }
       public Button MoveFreeCamLeft { get; set; }
       public Axis MoveFreeCamZ { get; set; }
       public Axis MoveFreeCamUpAxis { get; set; }
       public Axis MoveFreeCamDownAxis { get; set; }
       public Button MoveFreeCamUp { get; set; }
       public Button MoveFreeCamDown { get; set; }
       public Setting<string> PitchCameraMouse { get; set; } // TODO: enum?
       public Setting<string> YawCameraMouse { get; set; } // TODO: enum?
       public Axis PitchCamera { get; set; }
       public Setting<decimal> FreeCamMouseSensitivity { get; set; }
       public Setting<bool> FreeCamMouseYDecay { get; set; } // TODO: check data type
       public Button PitchCameraUp { get; set; }
       public Button PitchCameraDown { get; set; }
       public Axis YawCamera { get; set; }
       public Setting<bool> FreeCamMouseXDecay { get; set; } // TODO: check data type
       public Button YawCameraLeft { get; set; }
       public Button YawCameraRight { get; set; }
       public Axis RollCamera { get; set; }
       public Button RollCameraLeft { get; set; }
       public Button RollCameraRight { get; set; }
       public Button ToggleRotationLock { get; set; }
       public Button FixCameraRelativeToggle { get; set; }
       public Button FixCameraWorldToggle { get; set; }
       public Button QuitCamera { get; set; }
       public Button ToggleAdvanceMode { get; set; }
       public Button FreeCamZoomIn { get; set; }
       public Button FreeCamZoomOut { get; set; }
       public Button FStopDec { get; set; }
       public Button FStopInc { get; set; }
       public Button CommanderCreator_Undo { get; set; }
       public Button CommanderCreator_Redo { get; set; }
       public Button CommanderCreator_Rotation_MouseToggle { get; set; }
       public Axis CommanderCreator_Rotation { get; set; }
       public Button GalnetAudio_Play_Pause { get; set; }
       public Button GalnetAudio_SkipForward { get; set; }
       public Button GalnetAudio_SkipBackward { get; set; }
       public Button GalnetAudio_ClearQueue { get; set; }
       public Axis ExplorationFSSCameraPitch { get; set; }
       public Button ExplorationFSSCameraPitchIncreaseButton { get; set; }
       public Button ExplorationFSSCameraPitchDecreaseButton { get; set; }
       public Axis ExplorationFSSCameraYaw { get; set; }
       public Button ExplorationFSSCameraYawIncreaseButton { get; set; }
       public Button ExplorationFSSCameraYawDecreaseButton { get; set; }
       public Button ExplorationFSSZoomIn { get; set; }
       public Button ExplorationFSSZoomOut { get; set; }
       public Button ExplorationFSSMiniZoomIn { get; set; }
       public Button ExplorationFSSMiniZoomOut { get; set; }
       public Axis ExplorationFSSRadioTuningX_Raw { get; set; }
       public Button ExplorationFSSRadioTuningX_Increase { get; set; }
       public Button ExplorationFSSRadioTuningX_Decrease { get; set; }
       public Axis ExplorationFSSRadioTuningAbsoluteX { get; set; }
       public Setting<decimal> FSSTuningSensitivity { get; set; }
       public Button ExplorationFSSDiscoveryScan { get; set; }
       public Button ExplorationFSSQuit { get; set; }
       public Setting<string> FSSMouseXMode { get; set; } // TODO: enum?
       public Setting<bool> FSSMouseXDecay { get; set; } // TODO: check data type
       public Setting<string> FSSMouseYMode { get; set; } // TODO: enum?
       public Setting<bool> FSSMouseYDecay { get; set; } // TODO: check data type
       public Setting<decimal> FSSMouseSensitivity { get; set; }
       public Setting<decimal> FSSMouseDeadzone { get; set; }
       public Setting<decimal> FSSMouseLinearity { get; set; }
       public Button ExplorationFSSTarget { get; set; }
       public Button ExplorationFSSShowHelp { get; set; }
       public Button ExplorationSAAChangeScannedAreaViewToggle { get; set; }
       public Button ExplorationSAAExitThirdPerson { get; set; }
       public Setting<string> SAAThirdPersonMouseXMode { get; set; } // TODO: enum?
       public Setting<bool> SAAThirdPersonMouseXDecay { get; set; } // TODO: check data type
       public Setting<string> SAAThirdPersonMouseYMode { get; set; } // TODO: enum?
       public Setting<bool> SAAThirdPersonMouseYDecay { get; set; } // TODO: check data type
       public Setting<decimal> SAAThirdPersonMouseSensitivity { get; set; }
       public Axis SAAThirdPersonYawAxisRaw { get; set; }
       public Button SAAThirdPersonYawLeftButton { get; set; }
       public Button SAAThirdPersonYawRightButton { get; set; }
       public Axis SAAThirdPersonPitchAxisRaw { get; set; }
       public Button SAAThirdPersonPitchUpButton { get; set; }
       public Button SAAThirdPersonPitchDownButton { get; set; }
       public Axis SAAThirdPersonFovAxisRaw { get; set; }
       public Button SAAThirdPersonFovOutButton { get; set; }
       public Button SAAThirdPersonFovInButton { get; set; }
   }
}
