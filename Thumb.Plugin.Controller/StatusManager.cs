using System;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Status;

namespace Thumb.Plugin.Controller
{
    public class StatusManager : IJournalProcessor
    {
        private readonly JournalEntryRouter _entryRouter;
        private readonly ControllerStatus _status = new ControllerStatus();
        private readonly IJournalMonitorNotifier _notifier;
        private bool _updateRequired;

        public StatusManager(IJournalMonitorNotifier notifier)
        {
            _entryRouter = new JournalEntryRouter();

            _entryRouter.RegisterFor<Status>(ApplyStatus);
            _notifier = notifier;

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

        public bool Apply(IJournalEntry journalEntry)
        {
            return _entryRouter.Apply(journalEntry);
        }

        public void Flush()
        {
            if (_updateRequired)
            {
                _notifier.UpdatedService(_status);
                _updateRequired = false;
            }
        }

        public void ActivateBinding(ControlRequest controlRequest)
        {
            Console.WriteLine($"Pressed a key: {controlRequest.BindingName}");
        }


    }
}