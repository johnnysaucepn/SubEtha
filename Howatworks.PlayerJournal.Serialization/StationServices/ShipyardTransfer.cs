namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ShipyardTransfer : JournalEntryBase
    {
        public string ShipType { get; set; }
        public int ShipID { get; set; }
        public string System { get; set; }
        public decimal Distance { get; set; }
        public int TransferPrice { get; set; }
    }
}
