namespace Howatworks.PlayerJournal.Travel
{
    public class Docked : JournalEntryBase
    {
        public string StationName { get; set; }
        public string StationType { get; set; }
        public string StarSystem { get; set; }
        public bool CockpitBreach { get; set; }
        public string StationFaction { get; set; }
        public string FactionState { get; set; }
        public string StationAllegiance { get; set; }
        public string StationEconomy { get; set; }
        public string StationEconomy_Localised { get; set; }
        public string StationGovernment { get; set; }
        public string StationGovernment_Localised { get; set; }
    }
}
