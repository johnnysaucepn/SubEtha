﻿using Howatworks.PlayerJournal.Parser;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Combat;
using Howatworks.PlayerJournal.Serialization.Other;
using Howatworks.PlayerJournal.Serialization.Travel;
using SubEtha.Domain;

namespace Thumb.Plugin.SubEtha
{
    public class LocationManager : IJournalProcessor
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
            Updated(location);

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
            Updated(fsdJump);

            return true;
        }

        private bool ApplyDocked(Docked docked)
        {
            if (_location.Body != null) _location.Body.Docked = true;
            _location.SurfaceLocation = null;
            _location.Station = Station.Create(docked.StationName, docked.StationType);
            _location.SignalSource = null;
            Updated(docked);
            return true;
        }

        private bool ApplyUndocked(Undocked undocked)
        {
            if (_location.Body != null) _location.Body.Docked = false;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            Updated(undocked);
            return true;
        }

        private bool ApplyTouchdown(Touchdown touchdown)
        {
            _location.Body = new Body(_location.Body.Name, _location.Body.Type);
            _location.SurfaceLocation = new SurfaceLocation(true, touchdown.Latitude, touchdown.Longitude);
            _location.Station = null;
            _location.SignalSource = null;
            Updated(touchdown);
            return true;
        }

        private bool ApplyLiftoff(Liftoff liftoff)
        {
            _location.Body = new Body(_location.Body.Name, _location.Body.Type);
            _location.SurfaceLocation = new SurfaceLocation(false, liftoff.Latitude, liftoff.Longitude);
            _location.Station = null;
            _location.SignalSource = null;
            Updated(liftoff);
            return true;
        }

        private bool ApplySuperCruiseEntry(SupercruiseEntry entry)
        {
            _location.Body = null;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            Updated(entry);
            return true;
        }

        private bool ApplySupercruiseExit(SupercruiseExit exit)
        {
            _location.Body = new Body(exit.Body, exit.BodyType);
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = null;
            Updated(exit);
            return true;
        }

        private bool ApplyUssDrop(UssDrop ussDrop)
        {
            _location.Body = null;
            _location.SurfaceLocation = null;
            _location.Station = null;
            _location.SignalSource = new SignalSource(new LocalisedString(ussDrop.USSType, ussDrop.USSType_Localised), ussDrop.USSThreat);
            Updated(ussDrop);
            return true;
        }

        private bool ApplyDied(Died died)
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

        public void Flush()
        {
            if (!_isDirty) return;

            _client.Upload(_location);
            _isDirty = false;
        }

    }
}
