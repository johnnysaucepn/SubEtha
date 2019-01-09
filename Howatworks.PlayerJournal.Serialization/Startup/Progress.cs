namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Progress : JournalEntryBase
    {
        public decimal Combat { get; set; } // Note: sample suggests int
        public decimal Trade { get; set; } // Note: sample suggests int
        public decimal Explore { get; set; } // Note: sample suggests int
        public decimal Empire { get; set; } // Note: sample suggests int
        public decimal Federation { get; set; } // Note: sample suggests int
        public decimal CQC { get; set; } // Note: sample suggests int
    }
}
