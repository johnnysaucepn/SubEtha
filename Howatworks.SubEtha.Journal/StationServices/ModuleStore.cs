using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class ModuleStore : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Slot { get; set; }
        public string Ship { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
        public string StoredItem { get; set; }
        public string StoredItem_Localised { get; set; }
        public bool Hot { get; set; }
        public string EngineerModifications { get; set; }
        public int Level { get; set; }
        public decimal Quality { get; set; }
        public string ReplacementItem { get; set; }
        public string ReplacementItem_Localised { get; set; }
        public long? Cost { get; set; }
    }
}
