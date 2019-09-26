using System;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Other;
using Howatworks.SubEtha.Journal.Travel;
using Howatworks.Thumb.Core;
using log4net;
using System.Collections.Concurrent;

namespace Howatworks.Thumb.Matrix.Core
{
    public class LocationManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LocationManager));

        private readonly CommanderTracker _commander;
        private readonly UploadQueue<LocationState> _client;
        private readonly ConcurrentDictionary<GameContext, LocationState> _locations = new ConcurrentDictionary<GameContext, LocationState>();
        private bool _isDirty;

        public LocationManager(JournalEntryRouter router, CommanderTracker commander, UploadQueue<LocationState> client)
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

            return Replace(location.Timestamp, new LocationState
            {
                StarSystem = new StarSystem(location.StarSystem, location.StarPos),
                Body = new Body(location.Body, location.BodyType, location.Docked),
                Station = Station.Create(location.StationName, location.StationType)
                // All other items set to default
            });
        }

        private bool ApplyFsdJump(FsdJump fsdJump)
        {
            // Ignore previous information, return new location
            return Replace(fsdJump.Timestamp, new LocationState
            {
                StarSystem = new StarSystem(fsdJump.StarSystem, fsdJump.StarPos)
                // All other items set to default
            });
        }

        private bool ApplyDocked(Docked docked)
        {
            return Modify(docked.Timestamp, x =>
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
            return Modify(undocked.Timestamp, x =>
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
            return Modify(touchdown.Timestamp, x =>
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
            return Modify(liftoff.Timestamp, x =>
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
            return Modify(entry.Timestamp, x =>
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
            return Modify(exit.Timestamp, x =>
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
            return Modify(ussDrop.Timestamp, x =>
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

            return Replace(died.Timestamp, new LocationState());
        }

        private bool Replace(DateTimeOffset timestamp, LocationState newState)
        {
            var discriminator = _commander.GetContext();
            if (discriminator is null) return false;

            newState.TimeStamp = timestamp;
            _locations[discriminator] = newState;
            _isDirty = true;
            return true;
        }

        private bool Modify(DateTimeOffset timestamp, Func<LocationState, bool> stateChange)
        {
            var discriminator = _commander.GetContext();
            if (discriminator is null) return false;

            var state = _locations.ContainsKey(discriminator) ? _locations[discriminator] : new LocationState();

            // If handler didn't apply the change, don't update state
            if (!stateChange(state)) return false;

            state.TimeStamp = timestamp;
            _locations[discriminator] = state;
            _isDirty = true;
            return true;
        }

        private bool BatchComplete()
        {
            if (!_isDirty) return false;

            foreach (var context in _locations)
            {
                _client.Enqueue(context.Key.GameVersion, context.Key.CommanderName, context.Value);
            }
            _isDirty = false;

            // Always try to commit immediately
            _client.Flush();

            return true;
        }
    }
}
