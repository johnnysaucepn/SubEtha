namespace Howatworks.SubEtha.Journal.Exploration
{
    public class BuyExplorationData : JournalEntryBase
    {
        public string System { get; set; } // Note: name
        public long Cost { get; set; }
    }
}
