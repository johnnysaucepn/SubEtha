using System;
using System.Reactive.Linq;
using Howatworks.Matrix.Domain;
using Howatworks.SubEtha.Journal;
using Howatworks.SubEtha.Journal.Combat;
using Howatworks.SubEtha.Journal.Startup;
using Howatworks.SubEtha.Journal.StationServices;
using Howatworks.SubEtha.Monitor;
using log4net;

namespace Howatworks.Thumb.Matrix.Core
{
    public class ShipManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ShipManager));

        private readonly Tracker<ShipState> _tracker = new Tracker<ShipState>();
        public IObservable<ShipState> Observable => _tracker.Observable;

        public void SubscribeTo(IObservable<JournalEntry> observable)
        {
            observable.OfJournalType<LoadGame>().Subscribe(ApplyLoadGame);

            observable.OfJournalType<ShipyardNew>().Subscribe(ApplyShipyardNew);
            observable.OfJournalType<ShipyardSwap>().Subscribe(ApplyShipyardSwap);

            observable.OfJournalType<ShieldState>().Subscribe(ApplyShieldState);
            observable.OfJournalType<HullDamage>().Subscribe(ApplyHullDamage);
            observable.OfJournalType<Repair>().Subscribe(ApplyRepair);
            observable.OfJournalType<RepairAll>().Subscribe(ApplyRepairAll);
        }

        private void ApplyLoadGame(LoadGame loadGame)
        {
            _tracker.Modify(loadGame.Timestamp, ship =>
            {
                ship.Type = loadGame.Ship;
                ship.ShipId = loadGame.ShipID;
                ship.Name = loadGame.ShipName;
                ship.Ident = loadGame.ShipIdent;
            });
        }

        private void ApplyShipyardNew(ShipyardNew shipyardNew)
        {
            _tracker.Replace(shipyardNew.Timestamp, ship =>
            {
                ship.Type = shipyardNew.ShipType;
                ship.ShipId = shipyardNew.NewShipID;
            });
        }

        private void ApplyShipyardSwap(ShipyardSwap shipyardSwap)
        {
            _tracker.Modify(shipyardSwap.Timestamp, ship =>
            {
                ship.Type = shipyardSwap.ShipType;
                ship.ShipId = shipyardSwap.ShipID;
            });
        }

        private void ApplyHullDamage(HullDamage hullDamage)
        {
            _tracker.Modify(hullDamage.Timestamp, ship =>
            {
                ship.HullIntegrity = hullDamage.Health;
            });
        }

        private void ApplyShieldState(ShieldState shieldState)
        {
            _tracker.Modify(shieldState.Timestamp, ship =>
            {
                // If shield state was unknown before (i.e. null) we know it now
                ship.ShieldsUp = shieldState.ShieldsUp;
            });
        }

        private void ApplyRepair(Repair repair)
        {
            _tracker.Modify(repair.Timestamp, ship =>
            {
                if (repair.Item == "hull" || repair.Item == "all") ship.HullIntegrity = 1;
            });
        }

        private void ApplyRepairAll(RepairAll repair)
        {
            _tracker.Modify(repair.Timestamp, ship =>
            {
                ship.HullIntegrity = 1;
            });
        }
    }
}
