namespace Howatworks.PlayerJournal.Exploration
{
    public class MaterialCollected : JournalEntryBase
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
