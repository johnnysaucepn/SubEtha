namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class LoadGame : JournalEntryBase
    {
        public string Commander { get; set; } // NOTE: Commander name
        public string Ship { get; set; }
        public int ShipID { get; set; }
        public bool StartLanded { get; set; }
        public bool StartDead { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
        public int Credits { get; set; }
        public int Load { get; set; }
    }
}
