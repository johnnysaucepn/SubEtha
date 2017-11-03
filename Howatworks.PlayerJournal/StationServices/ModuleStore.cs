namespace Howatworks.PlayerJournal.StationServices
{
    public class ModuleStore : JournalEntryBase
    {
        public string Slot { get; set; }
        public string Ship { get; set; }
        public int ShipID { get; set; }
        public string StoredItem { get; set; }
        public string StoredItem_Localised { get; set; }
        public string EngineerModifications { get; set; }
        // TODO: not localised?
        public string ReplacementItem { get; set; }
        public string ReplacementItem_Localised { get; set; }
        public int Cost { get; set; }
    }
}
