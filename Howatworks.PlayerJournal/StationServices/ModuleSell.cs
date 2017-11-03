namespace Howatworks.PlayerJournal.StationServices
{
    public class ModuleSell : JournalEntryBase
    {
        public string Slot { get; set; }
        public string SellItem { get; set; }
        public string SellItem_Localised { get; set; }
        public int SellPrice { get; set; }
        public string Ship { get; set; }
        public int ShipID { get; set; }
    }
}
