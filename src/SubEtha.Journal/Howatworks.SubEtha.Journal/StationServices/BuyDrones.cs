namespace Howatworks.SubEtha.Journal.StationServices
{
    public class BuyDrones : JournalEntryBase
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public long BuyPrice { get; set; }
        public long TotalCost { get; set; }
    }
}
