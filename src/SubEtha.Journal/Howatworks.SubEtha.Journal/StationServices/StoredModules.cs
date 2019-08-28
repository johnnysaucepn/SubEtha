using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class StoredModules : JournalEntryBase
    {
        public class ModuleItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public string StarSystem { get; set; }
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long MarketID { get; set; }
            public int StorageSlot { get; set; }
            public long TransferCost { get; set; }
            public int TransferTime { get; set; } // NOTE: time in seconds
            public long BuyPrice { get; set; } // WARNING: not in docs
            public bool Hot { get; set; }
            public string EngineerModifications { get; set; }
            public int? Level { get; set; }
            public decimal? Quality { get; set; }
            public bool? InTransit { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string StarSystem { get; set; } // WARNING: not in docs
        public string StationName { get; set; } // WARNING: not in docs
        public List<ModuleItem> Items { get; set; }
    }
}
