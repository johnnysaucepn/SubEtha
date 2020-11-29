using System;
using Howatworks.SubEtha.Journal.Status;
using Howatworks.Assistant.Core.Messages;
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

        private readonly ControlStateModel _status = new ControlStateModel();

        private readonly Subject<ControlStateModel> _controlStateSubject = new Subject<ControlStateModel>();
        public IObservable<ControlStateModel> ControlStateObservable => _controlStateSubject.AsObservable();

        public void SubscribeTo(IObservable<JournalEntry> observable)
        {
            observable.OfJournalType<Status>().Subscribe(ApplyStatus);
        }

        private void ApplyStatus(Status status)
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

            if (_status.IsDirty)
            {
                _controlStateSubject.OnNext(_status);
                _status.IsDirty = false;
            }
        }

        public ControlState CreateControlStateMessage()
        {
            return _status.CreateControlStateMessage();
        }
    }
}