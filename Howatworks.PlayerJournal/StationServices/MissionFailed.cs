namespace Howatworks.PlayerJournal.StationServices
{
    public class MissionFailed : JournalEntryBase
    {
        // TODO: localised?
        public string Name { get; set; }
        public int MissionID { get; set; }

    }
}
