namespace Howatworks.SubEtha.Journal.Exploration
{
    // Note: no sample
    [JournalName("FSSAllBodiesFound")]
    public class FssAllBodiesFound : JournalEntryBase
    {
        public string SystemName { get; set; }
        public long SystemAddress { get; set; }
        public int Count { get; set; }
    }
}
