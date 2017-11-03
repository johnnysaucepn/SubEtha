namespace Howatworks.PlayerJournal.StationServices
{
    public class EngineerApply : JournalEntryBase
    {
        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        // No example in document - name of override effect
        public string Override { get; set; }
    }
}
