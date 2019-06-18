using System;
using System.Collections.Generic;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.SubEtha.Journal.StationServices;
using Howatworks.SubEtha.Parser;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class ShipManager
    {
        private readonly IUploader<ShipState> _client;
        // ReSharper disable once UnusedMember.Local

        private readonly Dictionary<string, ShipState> _ships = new Dictionary<string, ShipState>();
        private bool _isDirty;

        public ShipManager(JournalEntryRouter router, IUploader<ShipState> client)
        {
            _client = client;

            router.RegisterFor<LoadGame>(ApplyLoadGame);
            router.RegisterFor<ShipyardNew>(ApplyShipyardNew);
            router.RegisterFor<ShipyardSwap>(ApplyShipyardSwap);

            router.RegisterFor<ShieldState>(ApplyShieldState);
            router.RegisterFor<HullDamage>(ApplyHullDamage);
            router.RegisterFor<Repair>(ApplyRepair);
            router.RegisterFor<RepairAll>(ApplyRepairAll);

            router.RegisterEndBatch(BatchComplete);
        }

        private bool ApplyLoadGame(LoadGame loadGame, BatchMode mode)
        {
            return Apply(loadGame, ship =>
            {
                ship.Type = loadGame.Ship;
                ship.ShipID = loadGame.ShipID;
                //ship.Name = loadGame.ShipName;
                //ship.Ident = loadGame.ShipIdent;
                return true;
            });
        }

        private bool ApplyShipyardNew(ShipyardNew shipyardNew, BatchMode mode)
        {
            return Replace(shipyardNew, () => new ShipState
            {
                Type = shipyardNew.ShipType,
                ShipID = shipyardNew.NewShipID
            });
        }

        private bool ApplyShipyardSwap(ShipyardSwap shipyardSwap, BatchMode mode)
        {
            return Apply(shipyardSwap, ship =>
            {
                ship.Type = shipyardSwap.ShipType;
                ship.ShipID = shipyardSwap.ShipID;
                return true;
            });
        }

        private bool ApplyHullDamage(HullDamage hullDamage, BatchMode mode)
        {
            return Apply(hullDamage, ship =>
            {
                ship.HullIntegrity = hullDamage.Health;
                return true;
            });
        }

        private bool ApplyShieldState(ShieldState shieldState, BatchMode mode)
        {
            return Apply(shieldState, ship =>
            {
                // If shield state was unknown before (i.e. null) we know it now
                ship.ShieldsUp = shieldState.ShieldsUp;
                return true;
            });
        }

        private bool ApplyRepair(Repair repair, BatchMode mode)
        {
            return Apply(repair, ship =>
            {
                if (repair.Item != "hull" && repair.Item != "all") return false;
                ship.HullIntegrity = 1;
                return true;
            });
        }

        private bool ApplyRepairAll(RepairAll repair, BatchMode mode)
        {
            return Apply(repair, ship =>
            {
                ship.HullIntegrity = 1;
                return true;
            });
        }

        private bool BatchComplete(BatchMode mode)
        {
            if (!_isDirty) return false;

            foreach (ShipState ship in _ships.Values)
            {
                _client.Upload(ship);
            }

            _isDirty = false;
            return true;
        }

        private bool Apply(IJournalEntry entry, Func<ShipState, bool> action)
        {
            if (!_ships.ContainsKey(entry.GameVersionDiscriminator))
            {
                _ships[entry.GameVersionDiscriminator] = new ShipState();
            }

            var ship = _ships[entry.GameVersionDiscriminator];

            // If handler didn't apply the change, don't update state
            if (action(ship))
            {
                ship.TimeStamp = entry.Timestamp;
                _isDirty = true;
                return true;
            }

            return false;
        }

        private bool Replace(IJournalEntry entry, Func<ShipState> action)
        {
            // If handler didn't apply the change, don't update state
            var newState = action();
            if (newState != null)
            {
                newState.TimeStamp = entry.Timestamp;
                _isDirty = true;

                _ships[entry.GameVersionDiscriminator] = newState;
                return true;
            }

            return false;
        }

    }
}
