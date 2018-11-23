namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class FetchRemoteModule : JournalEntryBase
    {
        public string StorageSlot { get; set; }
        public string StoredItem { get; set; }
        // TODO: is this localised?
        public string StoredItem_Localised { get; set; }
        public int ServerId { get; set; }
        public int TransferCost { get; set; }
        public string Ship { get; set; }
        public int ShipID { get; set; }
    }
}
