using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class ModuleSellRemote : JournalEntryBase
    {
        public int StorageSlot { get; set; }
        public string SellItem { get; set; }
        public string SellItem_Localised { get; set; }
        public string ServerId { get; set; } // TODO: check data type and capitalisation
        public long SellPrice { get; set; }
        public string Ship { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; } // check capitalisation, doc says ShipId

    }
}
