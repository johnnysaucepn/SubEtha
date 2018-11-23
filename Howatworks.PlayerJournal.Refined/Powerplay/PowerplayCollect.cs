namespace Howatworks.PlayerJournal.Serialization.Powerplay
{
    public class PowerplayCollect : JournalEntryBase
    {
        public string Power { get; set; }
        // TODO: localised?
        public string Type { get; set; }
        public int Count { get; set; }
    }
}
