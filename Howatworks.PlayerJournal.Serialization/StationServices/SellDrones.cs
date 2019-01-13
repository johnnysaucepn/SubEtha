namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class SellDrones : JournalEntryBase
    {
        public string Type { get; set; } // TODO: localised?
        public int Count { get; set; }
        public int SellPrice { get; set; }
        public int TotalSale { get; set; }
    }
}
