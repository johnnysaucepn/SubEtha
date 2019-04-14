using System;
using Howatworks.SubEtha.Journal.Status;
using Howatworks.SubEtha.Parser;
using log4net;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public class StatusManager : IJournalProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StatusManager));

        private readonly ControlStateModel _status = new ControlStateModel();
        private bool _updateRequired;

        public event EventHandler<ControlStateModelChangedEventArgs> ControlStateChanged = (sender, args) => { };

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
                ControlStateChanged?.Invoke(this, new ControlStateModelChangedEventArgs(_status));
                _updateRequired = false;
            }
        }

    }
}