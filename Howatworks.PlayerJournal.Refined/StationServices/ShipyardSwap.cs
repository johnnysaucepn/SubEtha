namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ShipyardSwap : JournalEntryBase
    {
        public string ShipType { get; set; }
        public int ShipID { get; set; }
        public string StoreOldShip { get; set; }
        public int StoreShipID { get; set; }
        public string SellOldShip { get; set; }
        public int SellShipID { get; set; }
    }
}
