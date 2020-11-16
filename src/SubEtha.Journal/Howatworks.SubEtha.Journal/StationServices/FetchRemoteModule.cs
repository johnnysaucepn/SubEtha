using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    // Note: no sample
    [ExcludeFromCodeCoverage]
    public class FetchRemoteModule : JournalEntryBase
    {
        public int StorageSlot { get; set; }
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
