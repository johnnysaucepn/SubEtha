namespace Howatworks.SubEtha.Journal.Exploration
{
    // Note: no sample
    [JournalName("FSSDiscoveryScan")]
    public class FssDiscoveryScan : JournalEntryBase
    {
        public decimal Progress { get; set; }
        public int BodyCount { get; set; }
        public int NonBodyCount { get; set; }
    }
}
