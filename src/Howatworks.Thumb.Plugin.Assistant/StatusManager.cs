using System;
using Howatworks.SubEtha.Journal.Status;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Plugin.Assistant.Messages;
using log4net;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public class StatusManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StatusManager));

        private readonly ControlStateModel _status = new ControlStateModel();
        private bool _updateRequired;

        public event EventHandler<ControlStateModelChangedEventArgs> ControlStateChanged = (sender, args) => { };

        public StatusManager(JournalEntryRouter router)
        {
            router.RegisterFor<Status>(ApplyStatus, BatchPolicy.OnlyOngoing);

            router.RegisterForBatchComplete(BatchComplete, BatchPolicy.OnlyOngoing);

            _status.Changed += Status_Changed;
        }

        private void Status_Changed(object sender, EventArgs e)
        {
            _updateRequired = true;
        }

        private bool ApplyStatus(Status status)
        {
            _status.LandingGearDown = status.HasFlag(StatusFlags.LandingGearDown);
            _status.Supercruise = status.HasFlag(StatusFlags.Supercruise);
            _status.HardpointsDeployed = status.HasFlag(StatusFlags.HardPointsDeployed);
            _status.LightsOn = status.HasFlag(StatusFlags.LightsOn);
            _status.CargoScoopDeployed = status.HasFlag(StatusFlags.CargoScoopDeployed);
            _status.NightVision = status.HasFlag(StatusFlags.NightVision);
            _status.HudAnalysisMode = status.HasFlag(StatusFlags.HudAnalysisMode);
            _status.FssMode = status.GuiFocus == GuiFocus.FssMode;
            _status.SaaMode = status.GuiFocus == GuiFocus.SaaMode;

            return true;
        }

        public ControlState CreateControlStateMessage()
        {
            return CreateControlStateMessage(_status);
        }

        public static ControlState CreateControlStateMessage(ControlStateModel model)
        {
            return new ControlState
            {
                CargoScoopDeployed = model.CargoScoopDeployed,
                HardpointsDeployed = model.HardpointsDeployed,
                HudAnalysisMode = model.HudAnalysisMode,
                LandingGearDown = model.LandingGearDown,
                LightsOn = model.LightsOn,
                NightVision = model.NightVision,
                Supercruise = model.Supercruise,
                FssMode = model.FssMode,
                SaaMode = model.SaaMode
            };
        }

        private bool BatchComplete()
        {
            if (!_updateRequired) return false;

            ControlStateChanged?.Invoke(this, new ControlStateModelChangedEventArgs(_status));
            _updateRequired = false;
            return true;
        }

    }
}