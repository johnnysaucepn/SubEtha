namespace Howatworks.SubEtha.Journal.Travel
{
    [JournalName("FSDTarget")]
    public class FsdTarget : JournalEntryBase
    {
        public long SystemAddress { get; set; }
        //public string StarSystem { get; set; }
        public string Name { get; set; }
    }
}
