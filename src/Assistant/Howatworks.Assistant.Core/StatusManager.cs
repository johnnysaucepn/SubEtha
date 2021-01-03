using System;
using Howatworks.SubEtha.Journal.Status;
using log4net;
using System.Reactive.Subjects;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Monitor;
using System.Reactive.Linq;

namespace Howatworks.Assistant.Core
{
    public class StatusManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StatusManager));

        public ControlStateModel State { get; } = new ControlStateModel();

        private readonly Subject<ControlStateModel> _controlStateSubject = new Subject<ControlStateModel>();
        public IObservable<ControlStateModel> ControlStateObservable => _controlStateSubject.AsObservable();

        public void SubscribeTo(IObservable<JournalEntry> observable)
        {
            observable.OfJournalType<Status>().Subscribe(ApplyStatus);
        }

        private void ApplyStatus(Status status)
        {
            State.LandingGearDown = status.HasFlag(StatusFlags.LandingGearDown);
            State.Supercruise = status.HasFlag(StatusFlags.Supercruise);
            State.HardpointsDeployed = status.HasFlag(StatusFlags.HardPointsDeployed);
            State.LightsOn = status.HasFlag(StatusFlags.LightsOn);
            State.CargoScoopDeployed = status.HasFlag(StatusFlags.CargoScoopDeployed);
            State.NightVision = status.HasFlag(StatusFlags.NightVision);
            State.HudAnalysisMode = status.HasFlag(StatusFlags.HudAnalysisMode);
            State.FssMode = status.GuiFocus == GuiFocus.FssMode;
            State.SaaMode = status.GuiFocus == GuiFocus.SaaMode;

            if (State.IsDirty)
            {
                _controlStateSubject.OnNext(State);
                State.IsDirty = false;
            }
        }
    }
}