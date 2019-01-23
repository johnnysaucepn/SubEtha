namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Commander : JournalEntryBase
    {
        public string Name { get; set; }    // NOTE: Commander name
        public string FID { get; set; }     // NOTE: Player ID
    }
}
