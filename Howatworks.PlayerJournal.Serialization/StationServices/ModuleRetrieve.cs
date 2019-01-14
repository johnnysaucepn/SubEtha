using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class ModuleRetrieve : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Slot { get; set; }
        public string Ship { get; set; } // Note: ship type
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
        // TODO: localised?
        public string RetrievedItem { get; set; }
        public bool Hot { get; set; } // TODO: check data type
        public string EngineerModifications { get; set; } // TODO: localised?
        public int Level { get; set; }
        public decimal Quality { get; set; }
        public string SwapOutItem { get; set; }
        public long Cost { get; set; }
    }
}
