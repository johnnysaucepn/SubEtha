namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ModuleSwap : JournalEntryBase
    {
        public string FromSlot { get; set; }
        public string ToSlot { get; set; }
        public string FromItem { get; set; }
        public string FromItem_Localised { get; set; }
        // TODO: if null, not localised
        public string ToItem { get; set; }
        public string ToItem_Localised { get; set; }
        public string Ship { get; set; }
        public int ShipID { get; set; }
    }
}
