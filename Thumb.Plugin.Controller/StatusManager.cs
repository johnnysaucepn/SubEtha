using System;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization.Status;

namespace Thumb.Plugin.Controller
{
    public class StatusManager : IJournalProcessor
    {
        private readonly GameStatus _status = new GameStatus();
        private bool _updateRequired;

        public event EventHandler<ControllerModeUpdateEventArgs> ControllerModeUpdated = (sender, args) => { };

        public StatusManager(JournalEntryRouter router)
        {
            router.RegisterFor<Status>(ApplyStatus);

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

            return true;
        }

        public void Flush()
        {
            if (_updateRequired)
            {
                ControllerModeUpdated?.Invoke(this, new ControllerModeUpdateEventArgs(_status));
                _updateRequired = false;
            }
        }

        public void ActivateBinding(ControlRequest controlRequest)
        {
            Console.WriteLine($"Pressed a key: {controlRequest.BindingName}");
        }


    }
}