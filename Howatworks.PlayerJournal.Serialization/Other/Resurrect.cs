namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class Resurrect : JournalEntryBase
    {
        public string Option { get; set; } // TODO: enum?
        public long Cost { get; set; }
        public bool Bankrupt { get; set; }
    }
}
