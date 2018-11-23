namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class DockingDenied : JournalEntryBase
    {
        public string StationName { get; set; }
        public string Reason { get; set; }
    }
}
