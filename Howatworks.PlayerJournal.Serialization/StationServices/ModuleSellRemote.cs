using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ModuleSellRemote : JournalEntryBase
    {
        public string StorageSlot { get; set; }
        public string SellItem { get; set; }
        public string SellItem_Localised { get; set; }
        public string ServerId { get; set; } // TODO: check data type and capitalisation
        public int SellPrice { get; set; }
        public string Ship { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; } // check capitalisation, doc says ShipId

    }
}
