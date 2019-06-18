using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Other;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Travel;
using Howatworks.SubEtha.Monitor;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class LocationManager
    {
        private readonly IUploader<LocationState> _client;
        private LocationState _location;
        private bool _isDirty;

        public LocationManager(JournalEntryRouter router, IUploader<LocationState> client)
        {
            _client = client;

            _location = new LocationState();

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

            router.RegisterEndBatch(BatchComplete);
        }

        private bool ApplyLocation(Location location, BatchMode mode)
        {
            // Ignore previous information, return new location

            _location = new LocationState
            {
                StarSystem = new StarSystem(location.StarSystem, location.StarPos),
                Body = new Body(location.Body, location.BodyType, location.Docked),
                Station = Station.Create(location.StationName, location.StationType)
                // All other items set to default
            };
            Updated(location);

            return true;
        }

        private bool ApplyFsdJump(FsdJump fsdJump, BatchMode mode)
        {
            // Ignore previous information, return new location
            _location = new LocationState
            {
                StarSystem = new StarSystem(fsdJump.StarSystem, fsdJump.StarPos)
                // All other items set to default
            };
            Updated(fsdJump);

            return true;
        }

        private bool ApplyDocked(Docked docked, BatchMode mode)
        {
            if (_location.Body != null) _location.Body.Docked = true;
            _location.SurfaceLocation = null;
            _location.Station = Station.Create(docked.StationName, docked.StationType);
            _location.SignalSource = null;
            Updated(docked);
            return true;
        }

        private bool ApplyUndocked(Undocked undocked, BatchMode mode)
        {
            if (_location.Body != null) _location.Body.Docked = false;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            Updated(undocked);
            return true;
        }

        private bool ApplyTouchdown(Touchdown touchdown, BatchMode mode)
        {
            _location.Body = new Body(_location.Body.Name, _location.Body.Type);
            _location.SurfaceLocation = new SurfaceLocation(true, touchdown.Latitude, touchdown.Longitude);
            _location.Station = null;
            _location.SignalSource = null;
            Updated(touchdown);
            return true;
        }

        private bool ApplyLiftoff(Liftoff liftoff, BatchMode mode)
        {
            _location.Body = new Body(_location.Body.Name, _location.Body.Type);
            _location.SurfaceLocation = new SurfaceLocation(false, liftoff.Latitude, liftoff.Longitude);
            _location.Station = null;
            _location.SignalSource = null;
            Updated(liftoff);
            return true;
        }

        private bool ApplySuperCruiseEntry(SupercruiseEntry entry, BatchMode mode)
        {
            _location.Body = null;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            Updated(entry);
            return true;
        }

        private bool ApplySupercruiseExit(SupercruiseExit exit, BatchMode mode)
        {
            _location.Body = new Body(exit.Body, exit.BodyType);
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            Updated(exit);
            return true;
        }

        private bool ApplyUssDrop(UssDrop ussDrop, BatchMode mode)
        {
            _location.Body = null;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = new SignalSource(new LocalisedString(ussDrop.USSType, ussDrop.USSType_Localised), ussDrop.USSThreat);
            Updated(ussDrop);
            return true;
        }

        private bool ApplyDied(Died died, BatchMode mode)
        {
            // Ignore previous information, return new location

            _location = new LocationState();
            Updated(died);
            return true;
        }

        private void Updated(IJournalEntry entry)
        {
            _location.TimeStamp = entry.Timestamp;
            _isDirty = true;
        }

        private bool BatchComplete(BatchMode mode)
        {
            if (!_isDirty) return false;

            _client.Upload(_location);
            _isDirty = false;
            return true;
        }

    }
}
