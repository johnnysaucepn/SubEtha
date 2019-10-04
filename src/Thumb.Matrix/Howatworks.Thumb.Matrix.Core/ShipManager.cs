using System;
using System.Collections.Concurrent;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.SubEtha.Journal.StationServices;
using Howatworks.Thumb.Core;
using log4net;

namespace Howatworks.Thumb.Matrix.Core
{
    public class ShipManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ShipManager));

        private readonly UploadQueue<ShipState> _queue;
        private readonly Tracker<ShipState> _tracker;

        public ShipManager(JournalEntryRouter router, CommanderTracker commander, UploadQueue<ShipState> queue)
        {
            _tracker = new Tracker<ShipState>(commander);
            _queue = queue;

            router.RegisterFor<LoadGame>(ApplyLoadGame);
            router.RegisterFor<ShipyardNew>(ApplyShipyardNew);
            router.RegisterFor<ShipyardSwap>(ApplyShipyardSwap);

            router.RegisterFor<ShieldState>(ApplyShieldState);
            router.RegisterFor<HullDamage>(ApplyHullDamage);
            router.RegisterFor<Repair>(ApplyRepair);
            router.RegisterFor<RepairAll>(ApplyRepairAll);

            router.RegisterForBatchComplete(BatchComplete);
        }

        public void FlushQueue()
        {
            _queue.Flush();
        }

        private bool ApplyLoadGame(LoadGame loadGame)
        {
            return _tracker.Modify(loadGame.Timestamp, ship =>
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
            return _tracker.Replace(shipyardNew.Timestamp, ship =>
            {
                ship.Type = shipyardNew.ShipType;
                ship.ShipId = shipyardNew.NewShipID;
                return true;
            });
        }

        private bool ApplyShipyardSwap(ShipyardSwap shipyardSwap)
        {
            return _tracker.Modify(shipyardSwap.Timestamp, ship =>
            {
                ship.Type = shipyardSwap.ShipType;
                ship.ShipId = shipyardSwap.ShipID;
                return true;
            });
        }

        private bool ApplyHullDamage(HullDamage hullDamage)
        {
            return _tracker.Modify(hullDamage.Timestamp, ship =>
            {
                ship.HullIntegrity = hullDamage.Health;
                return true;
            });
        }

        private bool ApplyShieldState(ShieldState shieldState)
        {
            return _tracker.Modify(shieldState.Timestamp, ship =>
            {
                // If shield state was unknown before (i.e. null) we know it now
                ship.ShieldsUp = shieldState.ShieldsUp;
                return true;
            });
        }

        private bool ApplyRepair(Repair repair)
        {
            return _tracker.Modify(repair.Timestamp, ship =>
            {
                if (repair.Item != "hull" && repair.Item != "all") return false;
                ship.HullIntegrity = 1;
                return true;
            });
        }

        private bool ApplyRepairAll(RepairAll repair)
        {
            return _tracker.Modify(repair.Timestamp, ship =>
            {
                ship.HullIntegrity = 1;
                return true;
            });
        }

        private bool BatchComplete()
        {
            _tracker.Commit(() => { _queue.Enqueue(_tracker.GameVersion, _tracker.CommanderName, _tracker.CurrentState); });

            return true;
        }
    }
}
