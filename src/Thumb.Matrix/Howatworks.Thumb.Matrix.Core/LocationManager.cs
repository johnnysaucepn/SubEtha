using System;
using System.Reactive.Linq;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Other;
using Howatworks.SubEtha.Journal.Travel;
using log4net;
using Howatworks.SubEtha.Monitor;

namespace Howatworks.Thumb.Matrix.Core
{
    public class LocationManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationManager));

        private readonly Tracker<LocationState> _tracker = new Tracker<LocationState>();
        public IObservable<LocationState> Observable =>_tracker.Observable;

        public void SubscribeTo(IObservable<JournalEntry> observable)
        {
            observable.OfJournalType<Location>().Subscribe(ApplyLocation);
            observable.OfJournalType<FsdJump>().Subscribe(ApplyFsdJump);
            observable.OfJournalType<Docked>().Subscribe(ApplyDocked);
            observable.OfJournalType<Undocked>().Subscribe(ApplyUndocked);
            observable.OfJournalType<Touchdown>().Subscribe(ApplyTouchdown);
            observable.OfJournalType<Liftoff>().Subscribe(ApplyLiftoff);
            observable.OfJournalType<SupercruiseEntry>().Subscribe(ApplySuperCruiseEntry);
            observable.OfJournalType<SupercruiseExit>().Subscribe(ApplySupercruiseExit);
            observable.OfJournalType<UssDrop>().Subscribe(ApplyUssDrop);
            observable.OfJournalType<Died>().Subscribe(ApplyDied);
        }

        private void ApplyLocation(Location location)
        {
            _tracker.Replace(location.Timestamp, x =>
            {
                x.StarSystem = new StarSystem(location.StarSystem, location.StarPos);
                x.Body = new Body(location.Body, location.BodyType, location.Docked);
                x.Station = Station.Create(location.StationName, location.StationType);
                // All other items set to default
                return true;
            });
        }

        private void ApplyFsdJump(FsdJump fsdJump)
        {
            _tracker.Replace(fsdJump.Timestamp, x=>
            {
                x.StarSystem = new StarSystem(fsdJump.StarSystem, fsdJump.StarPos);
                // All other items set to default
                return true;
            });
        }

        private void ApplyDocked(Docked docked)
        {
            _tracker.Modify(docked.Timestamp, x =>
            {
                if (x.Body != null) x.Body.Docked = true;
                x.SurfaceLocation = null;
                x.Station = Station.Create(docked.StationName, docked.StationType);
                x.SignalSource = null;
                return true;
            });
        }

        private void ApplyUndocked(Undocked undocked)
        {
            _tracker.Modify(undocked.Timestamp, x =>
            {
                if (x.Body != null) x.Body.Docked = false;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private void ApplyTouchdown(Touchdown touchdown)
        {
            _tracker.Modify(touchdown.Timestamp, x =>
            {
                x.Body = new Body(x.Body.Name, x.Body.Type);
                x.SurfaceLocation = new SurfaceLocation(true, touchdown.Latitude, touchdown.Longitude);
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private void ApplyLiftoff(Liftoff liftoff)
        {
            _tracker.Modify(liftoff.Timestamp, x =>
            {
                x.Body = new Body(x.Body.Name, x.Body.Type);
                x.SurfaceLocation = new SurfaceLocation(false, liftoff.Latitude, liftoff.Longitude);
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private void ApplySuperCruiseEntry(SupercruiseEntry entry)
        {
            _tracker.Modify(entry.Timestamp, x =>
            {
                x.Body = null;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private void ApplySupercruiseExit(SupercruiseExit exit)
        {
            _tracker.Modify(exit.Timestamp, x =>
            {
                x.Body = new Body(exit.Body, exit.BodyType);
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private void ApplyUssDrop(UssDrop ussDrop)
        {
            _tracker.Modify(ussDrop.Timestamp, x =>
            {
                x.Body = null;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = new SignalSource(new LocalisedString(ussDrop.USSType, ussDrop.USSType_Localised), ussDrop.USSThreat);
                return true;
            });
        }

        private void ApplyDied(Died died)
        {
            // Ignore previous information, return new location

            _tracker.Replace(died.Timestamp, x => true);
        }        
    }
}
