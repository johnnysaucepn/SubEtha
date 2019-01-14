using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    // Note: no sample
    public class FetchRemoteModule : JournalEntryBase
    {
        public string StorageSlot { get; set; }
        public string StoredItem { get; set; }
        // TODO: is this localised?
        public string StoredItem_Localised { get; set; }
        public string ServerId { get; set; } // TODO: check capitalisation and data type
        public long TransferCost { get; set; }
        public string Ship { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; } // TODO: check capitalisation
        public int TransferTime { get; set; } //TODO: check data type
    }
}
