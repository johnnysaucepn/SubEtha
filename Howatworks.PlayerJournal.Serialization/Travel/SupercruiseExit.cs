namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class SupercruiseExit : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public string Body { get; set; } // Note: name
        public string BodyID { get; set; } // TODO: check data type
        public string BodyType { get; set; }
    }
}
