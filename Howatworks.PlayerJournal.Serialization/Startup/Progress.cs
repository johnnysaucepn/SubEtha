namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Progress : JournalEntryBase
    {
        public decimal Combat { get; set; }
        public decimal Trade { get; set; }
        public decimal Explore { get; set; }
        public decimal Empire { get; set; }
        public decimal Federation { get; set; }
        public decimal CQC { get; set; }
    }
}
