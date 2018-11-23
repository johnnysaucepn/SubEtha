namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class EngineerProgress : JournalEntryBase
    {
        public string Engineer { get; set; }
        public int Rank { get; set; }
        // TODO: possible enum
        public string Progress { get; set; }
    }
}
