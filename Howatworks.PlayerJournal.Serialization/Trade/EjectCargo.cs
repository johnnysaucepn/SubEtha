namespace Howatworks.PlayerJournal.Serialization.Trade
{
    public class EjectCargo : JournalEntryBase
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public bool Abandoned { get; set; }
        public string PowerplayOrigin { get; set; }
    }
}
