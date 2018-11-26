using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Combat;
using Howatworks.PlayerJournal.Serialization.Other;
using Howatworks.PlayerJournal.Serialization.Travel;
using SubEtha.Domain;

namespace Thumb.Plugin.SubEtha
{
    public class LocationManager : IJournalProcessor
    {
        private readonly JournalEntryRouter _entryRouter;
        private readonly IUploader<LocationState> _client;
        private LocationState _location;
        private bool _isDirty;

        public LocationManager(IUploader<LocationState> client)
        {
            _entryRouter = new JournalEntryRouter();
            _client = client;

            _location = new LocationState();

            _entryRouter.RegisterFor<Location>(ApplyLocation);
            _entryRouter.RegisterFor<FsdJump>(ApplyFsdJump);
            _entryRouter.RegisterFor<Docked>(ApplyDocked);
            _entryRouter.RegisterFor<Undocked>(ApplyUndocked);
            _entryRouter.RegisterFor<Touchdown>(ApplyTouchdown);
            _entryRouter.RegisterFor<Liftoff>(ApplyLiftoff);
            _entryRouter.RegisterFor<SupercruiseEntry>(ApplySuperCruiseEntry);
            _entryRouter.RegisterFor<SupercruiseExit>(ApplySupercruiseExit);
            _entryRouter.RegisterFor<UssDrop>(ApplyUssDrop);
            _entryRouter.RegisterFor<Died>(ApplyDied);
        }

        private bool ApplyLocation(Location location)
        {
            // Ignore previous information, return new location

            _location = new LocationState
            {
                StarSystem = new StarSystem(location.StarSystem, location.StarPos),
                Body = new Body(location.Body, location.BodyType, location.Docked),
                Station = Station.Create(location.StationName, location.StationType)
                // All other items set to default
            };

            return true;
        }

        private bool ApplyFsdJump(FsdJump fsdJump)
        {
            // Ignore previous information, return new location
            _location = new LocationState
            {
                StarSystem = new StarSystem(fsdJump.StarSystem, fsdJump.StarPos)
                // All other items set to default
            };

            return true;
        }

        private bool ApplyDocked(Docked docked)
        {
            if (_location.Body != null) _location.Body.Docked = true;
            _location.SurfaceLocation = null;
            _location.Station = Station.Create(docked.StationName, docked.StationType);
            _location.SignalSource = null;
            return true;
        }

        private bool ApplyUndocked(Undocked undocked)
        {
            if (_location.Body != null) _location.Body.Docked = false;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            return true;
        }

        private bool ApplyTouchdown(Touchdown touchdown)
        {
            _location.Body = new Body(_location.Body.Name, _location.Body.Type);
            _location.SurfaceLocation = new SurfaceLocation(true, touchdown.Latitude, touchdown.Longitude);
            _location.Station = null;
            _location.SignalSource = null;
            return true;
        }

        private bool ApplyLiftoff(Liftoff liftoff)
        {
            _location.Body = new Body(_location.Body.Name, _location.Body.Type);
            _location.SurfaceLocation = new SurfaceLocation(false, liftoff.Latitude, liftoff.Longitude);
            _location.Station = null;
            _location.SignalSource = null;
            return true;
        }

        private bool ApplySuperCruiseEntry(SupercruiseEntry entry)
        {
            _location.Body = null;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            return true;
        }

        private bool ApplySupercruiseExit(SupercruiseExit exit)
        {
            _location.Body = new Body(exit.Body, exit.BodyType);
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            return true;
        }

        private bool ApplyUssDrop(UssDrop ussDrop)
        {
            _location.Body = null;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = new SignalSource(new LocalisedString(ussDrop.USSType, ussDrop.USSType_Localised), ussDrop.USSThreat);
            return true;
        }

        private bool ApplyDied(Died died)
        {
            // Ignore previous information, return new location

            _location = new LocationState();
            return true;
        }

        public bool Apply(IJournalEntry journalEntry)
        {
            if (!_entryRouter.Apply(journalEntry)) return false;
            _location.TimeStamp = journalEntry.Timestamp;
            _isDirty = true;
            return true;
        }

        public void Flush()
        {
            if (!_isDirty) return;

            _client.Upload(_location);
            _isDirty = false;
        }

    }
}
