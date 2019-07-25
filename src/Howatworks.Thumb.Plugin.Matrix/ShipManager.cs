using System;
using System.Collections.Generic;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.SubEtha.Journal.StationServices;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class ShipManager
    {
        private readonly CommanderTracker _commander;
        private readonly IUploader<ShipState> _client;
        private readonly Dictionary<GameContext, ShipState> _ships = new Dictionary<GameContext, ShipState>();
        private bool _isDirty;

        public ShipManager(JournalEntryRouter router, CommanderTracker commander, IUploader<ShipState> client)
        {
            _commander = commander;
            _client = client;

            router.RegisterFor<LoadGame>(ApplyLoadGame);
            router.RegisterFor<ShipyardNew>(ApplyShipyardNew);
            router.RegisterFor<ShipyardSwap>(ApplyShipyardSwap);

            router.RegisterFor<ShieldState>(ApplyShieldState);
            router.RegisterFor<HullDamage>(ApplyHullDamage);
            router.RegisterFor<Repair>(ApplyRepair);
            router.RegisterFor<RepairAll>(ApplyRepairAll);

            router.RegisterForBatchComplete(BatchComplete);
        }

        private bool ApplyLoadGame(LoadGame loadGame)
        {
            return Apply(loadGame.Timestamp, ship =>
            {
                ship.Type = loadGame.Ship;
                ship.ShipId = loadGame.ShipID;
                ship.Name = loadGame.ShipName;
                ship.Ident = loadGame.ShipIdent;
                return true;
            });
        }

        private bool ApplyShipyardNew(ShipyardNew shipyardNew)
        {
            return Replace(shipyardNew.Timestamp, new ShipState
            {
                Type = shipyardNew.ShipType,
                ShipId = shipyardNew.NewShipID
            });
        }

        private bool ApplyShipyardSwap(ShipyardSwap shipyardSwap)
        {
            return Apply(shipyardSwap.Timestamp, ship =>
            {
                ship.Type = shipyardSwap.ShipType;
                ship.ShipId = shipyardSwap.ShipID;
                return true;
            });
        }

        private bool ApplyHullDamage(HullDamage hullDamage)
        {
            return Apply(hullDamage.Timestamp, ship =>
            {
                ship.HullIntegrity = hullDamage.Health;
                return true;
            });
        }

        private bool ApplyShieldState(ShieldState shieldState)
        {
            return Apply(shieldState.Timestamp, ship =>
            {
                // If shield state was unknown before (i.e. null) we know it now
                ship.ShieldsUp = shieldState.ShieldsUp;
                return true;
            });
        }

        private bool ApplyRepair(Repair repair)
        {
            return Apply(repair.Timestamp, ship =>
            {
                if (repair.Item != "hull" && repair.Item != "all") return false;
                ship.HullIntegrity = 1;
                return true;
            });
        }

        private bool ApplyRepairAll(RepairAll repair)
        {
            return Apply(repair.Timestamp, ship =>
            {
                ship.HullIntegrity = 1;
                return true;
            });
        }

        private bool BatchComplete()
        {
            if (!_isDirty) return false;

            foreach (var context in _ships.Keys)
            {
                _client.Upload(context, _ships[context]);
            }

            _isDirty = false;
            return true;
        }

        private bool Apply(DateTimeOffset timestamp, Func<ShipState, bool> action)
        {
            var discriminator = _commander.Context;

            var ship = _ships.ContainsKey(discriminator) ? _ships[discriminator] : new ShipState();

            // If handler didn't apply the change, don't update state
            if (!action(ship)) return false;

            ship.TimeStamp = timestamp;
            _ships[discriminator] = ship;
            _isDirty = true;
            return true;
        }

        private bool Replace(DateTimeOffset timestamp, ShipState newState)
        {
            var discriminator = _commander.Context;

            // If handler didn't apply the change, don't update state
            if (newState == null) return false;

            newState.TimeStamp = timestamp;
            _isDirty = true;

            _ships[discriminator] = newState;
            return true;
        }
    }
}
