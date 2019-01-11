namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class Liftoff : JournalEntryBase
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool PlayerControlled { get; set; }
    }
}
