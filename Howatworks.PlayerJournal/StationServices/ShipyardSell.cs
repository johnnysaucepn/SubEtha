namespace Howatworks.PlayerJournal.StationServices
{
    public class ShipyardSell : JournalEntryBase
    {
        public string ShipType { get; set; }
        public int SellShipID { get; set; }
        public int ShipPrice { get; set; }
        public string System { get; set; }
    }
}
