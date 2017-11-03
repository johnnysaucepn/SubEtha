namespace Howatworks.PlayerJournal.Travel
{
    public class Location : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public decimal[] StarPos { get; set; }
        public string Body { get; set; }
        public string BodyType { get; set; }
        public bool Docked { get; set; }     
        public string StationName { get; set; }
        public string StationType { get; set; }
        public string SystemFaction { get; set; }
        public string FactionState { get; set; }
        public string SystemAllegiance { get; set; }
        public string SystemEconomy { get; set; }
        public string SystemEconomy_Localised { get; set; }
        public string SystemGovernment { get; set; }
        public string SystemGovernment_Localised { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemSecurity_Localised { get; set; }
        public string[] Powers { get; set; }
        public string PowerplayState { get; set; }
    }
}
