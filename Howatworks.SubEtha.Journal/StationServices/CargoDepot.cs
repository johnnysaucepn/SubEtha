using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class CargoDepot : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MissionID { get; set; }

        public string UpdateType { get; set; } // TODO: enum "Collect", "Deliver", "WingUpdate"
        public string CargoType { get; set; }
        public int? Count { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long StartMarketID { get; set; } // TODO: check data type
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long EndMarketID { get; set; } // TODO: check data type
        public int ItemsCollected { get; set; }
        public int ItemsDelivered { get; set; }
        public int TotalItemsToDeliver { get; set; }
        public decimal Progress { get; set; } // TODO: check scale - 0-1 or %age? (ItemsCollected-ItemsDelivered)/TotalItemsToDeliver
    }
}
