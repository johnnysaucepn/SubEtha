using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ModuleSwap : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string FromSlot { get; set; }
        public string ToSlot { get; set; }
        public string FromItem { get; set; }
        public string FromItem_Localised { get; set; }
        public string ToItem { get; set; }
        public string ToItem_Localised { get; set; }
        public string Ship { get; set; } // NOTE: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
    }
}
