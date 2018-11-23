namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Rank : JournalEntryBase
    {
        public int Combat { get; set; }
        public int Trade { get; set; }
        public int Explore { get; set; }
        public int Empire { get; set; }
        public int Federation { get; set; }
        public int CQC { get; set; }
    }
}
