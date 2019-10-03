using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Other;
using Howatworks.SubEtha.Journal.Travel;
using Howatworks.Thumb.Core;
using log4net;

namespace Howatworks.Thumb.Matrix.Core
{
    public class LocationManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationManager));

        private readonly UploadQueue<LocationState> _queue;
        private readonly Tracker<LocationState> _tracker;

        public LocationManager(JournalEntryRouter router, CommanderTracker commander, UploadQueue<LocationState> queue)
        {
            _tracker = new Tracker<LocationState>(commander);
            _queue = queue;

            router.RegisterFor<Location>(ApplyLocation);
            router.RegisterFor<FsdJump>(ApplyFsdJump);
            router.RegisterFor<Docked>(ApplyDocked);
            router.RegisterFor<Undocked>(ApplyUndocked);
            router.RegisterFor<Touchdown>(ApplyTouchdown);
            router.RegisterFor<Liftoff>(ApplyLiftoff);
            router.RegisterFor<SupercruiseEntry>(ApplySuperCruiseEntry);
            router.RegisterFor<SupercruiseExit>(ApplySupercruiseExit);
            router.RegisterFor<UssDrop>(ApplyUssDrop);
            router.RegisterFor<Died>(ApplyDied);

            router.RegisterForBatchComplete(BatchComplete);
        }

        public void FlushQueue()
        {
            _queue.Flush();
        }

        private bool ApplyLocation(Location location)
        {
            return _tracker.Replace(location.Timestamp, x =>
            {
                x.StarSystem = new StarSystem(location.StarSystem, location.StarPos);
                x.Body = new Body(location.Body, location.BodyType, location.Docked);
                x.Station = Station.Create(location.StationName, location.StationType);
                // All other items set to default
                return true;
            });
        }

        private bool ApplyFsdJump(FsdJump fsdJump)
        {
            return _tracker.Replace(fsdJump.Timestamp, x=>
            {
                x.StarSystem = new StarSystem(fsdJump.StarSystem, fsdJump.StarPos);
                // All other items set to default
                return true;
            });
        }

        private bool ApplyDocked(Docked docked)
        {
            return _tracker.Modify(docked.Timestamp, x =>
            {
                if (x.Body != null) x.Body.Docked = true;
                x.SurfaceLocation = null;
                x.Station = Station.Create(docked.StationName, docked.StationType);
                x.SignalSource = null;
                return true;
            });
        }

        private bool ApplyUndocked(Undocked undocked)
        {
            return _tracker.Modify(undocked.Timestamp, x =>
            {
                if (x.Body != null) x.Body.Docked = false;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private bool ApplyTouchdown(Touchdown touchdown)
        {
            return _tracker.Modify(touchdown.Timestamp, x =>
            {
                x.Body = new Body(x.Body.Name, x.Body.Type);
                x.SurfaceLocation = new SurfaceLocation(true, touchdown.Latitude, touchdown.Longitude);
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private bool ApplyLiftoff(Liftoff liftoff)
        {
            return _tracker.Modify(liftoff.Timestamp, x =>
            {
                x.Body = new Body(x.Body.Name, x.Body.Type);
                x.SurfaceLocation = new SurfaceLocation(false, liftoff.Latitude, liftoff.Longitude);
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private bool ApplySuperCruiseEntry(SupercruiseEntry entry)
        {
            return _tracker.Modify(entry.Timestamp, x =>
            {
                x.Body = null;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private bool ApplySupercruiseExit(SupercruiseExit exit)
        {
            return _tracker.Modify(exit.Timestamp, x =>
            {
                x.Body = new Body(exit.Body, exit.BodyType);
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
        }

        private bool ApplyUssDrop(UssDrop ussDrop)
        {
            return _tracker.Modify(ussDrop.Timestamp, x =>
            {
                x.Body = null;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = new SignalSource(new LocalisedString(ussDrop.USSType, ussDrop.USSType_Localised), ussDrop.USSThreat);
                return true;
            });
        }

        private bool ApplyDied(Died died)
        {
            // Ignore previous information, return new location

            return _tracker.Replace(died.Timestamp, x => true);
        }

        private bool BatchComplete()
        {
            _tracker.Commit(() => { _queue.Enqueue(_tracker.GameVersion, _tracker.CommanderName, _tracker.CurrentState); });

            return true;
        }
    }
}
