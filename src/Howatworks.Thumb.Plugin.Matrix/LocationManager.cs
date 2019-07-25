using System;
using System.Collections.Generic;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Other;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Travel;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class LocationManager
    {
        private readonly CommanderTracker _commander;
        private readonly IUploader<LocationState> _client;
        private readonly Dictionary<GameContext, LocationState> _locations = new Dictionary<GameContext, LocationState>();
        private bool _isDirty;

        public LocationManager(JournalEntryRouter router, CommanderTracker commander, IUploader<LocationState> client)
        {
            _commander = commander;
            _client = client;

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

        private bool ApplyLocation(Location location)
        {
            // Ignore previous information, return new location

            Replace(location, new LocationState
            {
                StarSystem = new StarSystem(location.StarSystem, location.StarPos),
                Body = new Body(location.Body, location.BodyType, location.Docked),
                Station = Station.Create(location.StationName, location.StationType)
                // All other items set to default
            });

            return true;
        }

        private bool ApplyFsdJump(FsdJump fsdJump)
        {
            // Ignore previous information, return new location
            Replace(fsdJump, new LocationState
            {
                StarSystem = new StarSystem(fsdJump.StarSystem, fsdJump.StarPos)
                // All other items set to default
            });

            return true;
        }

        private bool ApplyDocked(Docked docked)
        {
            Modify(docked, x =>
            {
                if (x.Body != null) x.Body.Docked = true;
                x.SurfaceLocation = null;
                x.Station = Station.Create(docked.StationName, docked.StationType);
                x.SignalSource = null;
                return true;
            });
            return true;
        }

        private bool ApplyUndocked(Undocked undocked)
        {
            Modify(undocked, x =>
            {
                if (x.Body != null) x.Body.Docked = false;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
            return true;
        }

        private bool ApplyTouchdown(Touchdown touchdown)
        {
            Modify(touchdown, x =>
            {
                x.Body = new Body(x.Body.Name, x.Body.Type);
                x.SurfaceLocation = new SurfaceLocation(true, touchdown.Latitude, touchdown.Longitude);
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
            return true;
        }

        private bool ApplyLiftoff(Liftoff liftoff)
        {
            Modify(liftoff, x =>
            {
                x.Body = new Body(x.Body.Name, x.Body.Type);
                x.SurfaceLocation = new SurfaceLocation(false, liftoff.Latitude, liftoff.Longitude);
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
            return true;
        }

        private bool ApplySuperCruiseEntry(SupercruiseEntry entry)
        {
            Modify(entry, x =>
            {
                x.Body = null;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
            return true;
        }

        private bool ApplySupercruiseExit(SupercruiseExit exit)
        {
            Modify(exit, x =>
            {
                x.Body = new Body(exit.Body, exit.BodyType);
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = null;
                return true;
            });
            return true;
        }

        private bool ApplyUssDrop(UssDrop ussDrop)
        {
            Modify(ussDrop, x =>
            {
                x.Body = null;
                x.SurfaceLocation = null;
                x.Station = null;
                x.SignalSource = new SignalSource(new LocalisedString(ussDrop.USSType, ussDrop.USSType_Localised), ussDrop.USSThreat);
                return true;
            });
            return true;
        }

        private bool ApplyDied(Died died)
        {
            // Ignore previous information, return new location

            Replace(died, new LocationState());
            return true;
        }

        private void Replace(IJournalEntry entry, LocationState newState)
        {
            var discriminator = _commander.Context;

            newState.TimeStamp = entry.Timestamp;
            _locations[discriminator] = newState;
            _isDirty = true;
        }

        private void Modify(IJournalEntry entry, Func<LocationState, bool> stateChange)
        {
            var discriminator = _commander.Context;

            var state = !_locations.ContainsKey(discriminator) ? new LocationState() : _locations[discriminator];

            // If handler didn't apply the change, don't update state
            if (!stateChange(state)) return;

            state.TimeStamp = entry.Timestamp;
            _locations[discriminator] = state;
            _isDirty = true;
        }

        private bool BatchComplete()
        {
            if (!_isDirty) return false;

            foreach (var context in _locations.Keys)
            {
                if (!string.IsNullOrWhiteSpace(context.CommanderName))
                {
                    _client.Upload(context, _locations[context]);
                }
            }

            _isDirty = false;
            return true;
        }

    }
}
