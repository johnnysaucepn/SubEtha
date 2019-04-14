using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class ModuleRetrieve : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Slot { get; set; }
        public string Ship { get; set; } // Note: ship type
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
        public string RetrievedItem { get; set; }
        public string RetrievedItem_Localised { get; set; }
        public bool Hot { get; set; }
        public string EngineerModifications { get; set; }
        public int? Level { get; set; }
        public decimal? Quality { get; set; }
        public string SwapOutItem { get; set; }
        public string SwapOutItem_Localised { get; set; }
        public long? Cost { get; set; } // WARNING: not found in logs
    }
}
