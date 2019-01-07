using System;
using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Status;
using Howatworks.PlayerJournal.Serialization.Travel;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class StatusManager : IJournalProcessor
    {
        private readonly JournalEntryRouter _entryRouter;

        public bool LandingGearDown { get; private set; }
        public bool Supercruise { get; private set; }
        public bool HardpointsDeployed { get; private set; }
        public bool LightsOn { get; private set; }
        public bool CargoScoopDeployed { get; private set; }
        public bool NightVision { get; private set; }
        public bool HudAnalysisMode { get; private set; }

        public StatusManager()
        {
            _entryRouter = new JournalEntryRouter();

            _entryRouter.RegisterFor<Status>(ApplyStatus);
        }

        private bool ApplyStatus(Status status)
        {
            LandingGearDown = status.HasFlag(StatusFlags.LandingGearDown);
            Supercruise = status.HasFlag(StatusFlags.Supercruise);
            HardpointsDeployed = status.HasFlag(StatusFlags.HardPointsDeployed);
            LightsOn = status.HasFlag(StatusFlags.LightsOn);
            CargoScoopDeployed = status.HasFlag(StatusFlags.CargoScoopDeployed);
            NightVision = status.HasFlag(StatusFlags.NightVision);
            HudAnalysisMode = status.HasFlag(StatusFlags.HudAnalysisMode);

            Console.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));

            return true;
        }

        public bool Apply(IJournalEntry journalEntry)
        {
            return _entryRouter.Apply(journalEntry);
        }

        public void Flush()
        {
            //TODO: do something to report the latest state of the ship
        }
    }
}