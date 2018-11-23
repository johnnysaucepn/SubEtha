namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class MaterialDiscovered : JournalEntryBase
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
