namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class ClearSavedGame : JournalEntryBase
    {
        public string Name { get; set; }    // NOTE: Commander name
        public string FID { get; set; }     // NOTE: Player ID
    }
}
