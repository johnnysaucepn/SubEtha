namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class DockingGranted : JournalEntryBase
    {
        public string StationName { get; set; }
        public int LandingPad { get; set; }
    }
}
