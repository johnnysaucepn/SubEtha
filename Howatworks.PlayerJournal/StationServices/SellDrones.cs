namespace Howatworks.PlayerJournal.StationServices
{
    public class SellDrones : JournalEntryBase
    {
        // TODO: localised?
        public string Type { get; set; }
        public int Count { get; set; }
        public int SellPrice { get; set; }
        public int TotalSale { get; set; }
    }
}
