namespace Howatworks.PlayerJournal.Serialization.Travel
{
    [JournalName("FSDJump")]
    public class FsdJump : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public decimal[] StarPos { get; set; }
        // TODO: Report that Body is never populated
        //public string Body { get; set; }
        public decimal JumpDist { get; set; }
        public decimal FuelUsed { get; set; }
        public decimal FuelLevel { get; set; }
        public bool BoostUsed { get; set; }
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
