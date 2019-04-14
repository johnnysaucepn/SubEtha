using System;
using System.Collections.Generic;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.SubEtha.Journal.StationServices;
using Howatworks.SubEtha.Parser;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class ShipManager : IJournalProcessor
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
        }

        private bool ApplyLoadGame(LoadGame loadGame)
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

        private bool ApplyShipyardNew(ShipyardNew shipyardNew)
        {
            return Replace(shipyardNew, () => new ShipState
            {
                Type = shipyardNew.ShipType,
                ShipID = shipyardNew.NewShipID
            });
        }

        private bool ApplyShipyardSwap(ShipyardSwap shipyardSwap)
        {
            return Apply(shipyardSwap, ship =>
            {
                ship.Type = shipyardSwap.ShipType;
                ship.ShipID = shipyardSwap.ShipID;
                return true;
            });
        }

        private bool ApplyHullDamage(HullDamage hullDamage)
        {
            return Apply(hullDamage, ship =>
            {
                ship.HullIntegrity = hullDamage.Health;
                return true;
            });
        }

        private bool ApplyShieldState(ShieldState shieldState)
        {
            return Apply(shieldState, ship =>
            {
                // If shield state was unknown before (i.e. null) we know it now
                ship.ShieldsUp = shieldState.ShieldsUp;
                return true;
            });
        }

        private bool ApplyRepair(Repair repair)
        {
            return Apply(repair, ship =>
            {
                if (repair.Item != "hull" && repair.Item != "all") return false;
                ship.HullIntegrity = 1;
                return true;
            });
        }

        private bool ApplyRepairAll(RepairAll repair)
        {
            return Apply(repair, ship =>
            {
                ship.HullIntegrity = 1;
                return true;
            });
        }

        public void Flush()
        {
            if (!_isDirty) return;

            foreach (ShipState ship in _ships.Values)
            {
                _client.Upload(ship);
            }

            _isDirty = false;
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
