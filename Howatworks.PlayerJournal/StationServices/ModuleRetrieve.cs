namespace Howatworks.PlayerJournal.StationServices
{
    public class ModuleRetrieve : JournalEntryBase
    {
        public string Slot { get; set; }
        public string Ship { get; set; }
        public int ShipID { get; set; }
        // TODO: localised?
        public string RetrievedItem { get; set; }
        public string EngineerModifications { get; set; }
        // TODO: localised?
        public string SwapOutItem { get; set; }
        public int Cost { get; set; }
    }
}
