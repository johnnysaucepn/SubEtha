using Howatworks.PlayerJournal.Processing;
using Howatworks.PlayerJournal.Serialization;
using Howatworks.PlayerJournal.Serialization.Combat;
using Howatworks.PlayerJournal.Serialization.Startup;
using Howatworks.PlayerJournal.Serialization.StationServices;

namespace Thumb.Plugin.SubEtha
{
    public class ShipManager : IJournalProcessor
    {
        private readonly JournalEntryRouter _entryRouter;
        private readonly IUploader<ShipState> _client;
        // ReSharper disable once UnusedMember.Local

        private ShipState _ship;
        private bool _isDirty;

        public ShipManager(IUploader<ShipState> client)
        {
            _entryRouter = _entryRouter = new JournalEntryRouter();
            _client = client;

            _ship = new ShipState();

            _entryRouter.RegisterFor<LoadGame>(ApplyLoadGame);
            _entryRouter.RegisterFor<ShipyardNew>(ApplyShipyardNew);
            _entryRouter.RegisterFor<ShipyardSwap>(ApplyShipyardSwap);

            _entryRouter.RegisterFor<ShieldState>(ApplyShieldState);
            _entryRouter.RegisterFor<HullDamage>(ApplyHullDamage);
            _entryRouter.RegisterFor<Repair>(ApplyRepair);
            _entryRouter.RegisterFor<RepairAll>(ApplyRepairAll);
        }

        private bool ApplyLoadGame(LoadGame loadGame)
        {
            _ship.Type = loadGame.Ship;
            _ship.ShipID = loadGame.ShipID;
            //_ship.Name = loadGame.ShipName;
            //_ship.Ident = loadGame.ShipIdent;
            return true;
        }

        private bool ApplyShipyardNew(ShipyardNew shipyardNew)
        {
            _ship = new ShipState
            {
                Type = shipyardNew.ShipType,
                ShipID = shipyardNew.ShipID
            };
            return true;
        }

        private bool ApplyShipyardSwap(ShipyardSwap shipyardSwap)
        {
            _ship = new ShipState
            {
                Type = shipyardSwap.ShipType,
                ShipID = shipyardSwap.ShipID
            };
            return true;
        }

        private bool ApplyHullDamage(HullDamage hullDamage)
        {
            _ship.HullIntegrity = hullDamage.Health;
            return true;
        }

        private bool ApplyShieldState(ShieldState shieldState)
        {
            // If shield state was unknown before (i.e. null) we know it now
            _ship.ShieldsUp = shieldState.ShieldsUp;
            return true;
        }

        private bool ApplyRepair(Repair repair)
        {
            if (repair.Item != "hull" && repair.Item != "all") return false;
            _ship.HullIntegrity = 1;
            return true;
        }

        private bool ApplyRepairAll(RepairAll repair)
        {
            _ship.HullIntegrity = 1;
            return true;
        }

        public bool Apply(IJournalEntry journalEntry)
        {
            if (!_entryRouter.Apply(journalEntry)) return false;
            _ship.TimeStamp = journalEntry.Timestamp;
            _isDirty = true;
            return true;
        }


        public void Flush()
        {
            if (!_isDirty) return;

            _client.Upload(_ship);
            _isDirty = false;
        }
        
    }
}
