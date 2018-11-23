namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class MaterialDiscarded : JournalEntryBase
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
