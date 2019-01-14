namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class Resurrect : JournalEntryBase
    {
        // TODO: confirm type
        public string Option { get; set; }
        public long Cost { get; set; }
        public bool Bankrupt { get; set; }
    }
}
