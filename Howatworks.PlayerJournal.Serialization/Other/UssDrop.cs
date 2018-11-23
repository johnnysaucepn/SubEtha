namespace Howatworks.PlayerJournal.Serialization.Other
{
    [JournalName("USSDrop")]
    public class UssDrop : JournalEntryBase
    {
        public string USSType { get; set; }
        public string USSType_Localised { get; set; }
        public int USSThreat { get; set; }
    }
}
