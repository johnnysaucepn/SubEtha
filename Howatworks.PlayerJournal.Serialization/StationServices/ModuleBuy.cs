namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ModuleBuy : JournalEntryBase
    {
        public string Slot { get; set; }
        public string BuyItem { get; set; }
        public string BuyItem_Localised { get; set; }
        public int BuyPrice { get; set; }
        public string Ship { get; set; }
        public int ShipID { get; set; }

        public string SellItem { get; set; }
        public string SellItem_Localised { get; set; }
        public int SellPrice { get; set; }
    }
}
