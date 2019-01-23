using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Status;

namespace Thumb.Plugin.Controller
{
    public class StatusManager : IJournalProcessor
    {
        private readonly JournalEntryRouter _entryRouter;
        private readonly ControllerStatus _status;
        private readonly IJournalMonitorNotifier _notifier;

        public StatusManager(IJournalMonitorNotifier notifier)
        {
            _notifier = notifier;
            _status = new ControllerStatus(_notifier);
            _entryRouter = new JournalEntryRouter();

            _entryRouter.RegisterFor<Status>(ApplyStatus);
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
            // TODO: do a thing
            _status.Flush();
        }
    }
}