namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class MaterialCollected : JournalEntryBase
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
