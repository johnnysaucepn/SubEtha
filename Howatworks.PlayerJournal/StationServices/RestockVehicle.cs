namespace Howatworks.PlayerJournal.StationServices
{
    public class RestockVehicle : JournalEntryBase
    {
        public string Type { get; set; }
        public string Loadout { get; set; }
        public int Cost { get; set; }
        public int Count { get; set; }
    }
}
