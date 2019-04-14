namespace Howatworks.SubEtha.Journal.Exploration
{
    public class MaterialCollected : JournalEntryBase
    {
        public string Category { get; set; } // TODO: enum? Raw/Encoded/Manufactured
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public int Count { get; set; }
    }
}
