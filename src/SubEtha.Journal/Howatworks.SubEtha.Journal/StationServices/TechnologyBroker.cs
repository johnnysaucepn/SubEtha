using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class TechnologyBroker : JournalEntryBase
    {
        public class ItemsUnlockedItem
        {
            public string Name { get; set; }
        }

        public class CommoditiesItem
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }

        public class MaterialsItem
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public string Category { get; set; } // TODO: enum - raw, etc.
        }

        public string BrokerType { get; set; } // TODO: enum?
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public List<ItemsUnlockedItem> ItemsUnlocked { get; set; }
        public List<CommoditiesItem> Commodities { get; set; }
        public List<MaterialsItem> Materials { get; set; }
    }
}
