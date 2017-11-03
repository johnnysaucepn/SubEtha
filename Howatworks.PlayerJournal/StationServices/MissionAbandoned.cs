namespace Howatworks.PlayerJournal.StationServices
{
    public class MissionAbandoned : JournalEntryBase
    {
        // TODO: is this localised?
        public string Name { get; set; }
        public int MissionID { get; set; }
    }
}
