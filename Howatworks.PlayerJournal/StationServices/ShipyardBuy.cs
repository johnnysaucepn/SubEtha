namespace Howatworks.PlayerJournal.StationServices
{
    public class ShipyardBuy : JournalEntryBase
    {
        public string ShipType { get; set; }
        public int ShipPrice { get; set; }
        public string StoreOldShip { get; set; }
        public int StoreShipID { get; set; }
        public string SellOldShip { get; set; }
        public int SellShipID { get; set; }
        public int SellPrice { get; set; }
    }
}
