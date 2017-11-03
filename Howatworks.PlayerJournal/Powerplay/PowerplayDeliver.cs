namespace Howatworks.PlayerJournal.Powerplay
{
    public class PowerplayDeliver : JournalEntryBase
    {
        public string Power { get; set; }
        // TODO: localised?
        public string Type { get; set; }
        public int Count { get; set; }
    }
}
