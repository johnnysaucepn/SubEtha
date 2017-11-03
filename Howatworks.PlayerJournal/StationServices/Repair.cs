namespace Howatworks.PlayerJournal.StationServices
{
    public class Repair : JournalEntryBase
    {
        public string Item { get; set; }
        // TODO: optionally localised?
        //public string Item_Localised { get; set; }
        public int Cost { get; set; }
    }
}
