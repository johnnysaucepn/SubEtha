using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class StoredModules : JournalEntryBase
    {
        public class ModuleItem
        {
            public string Name { get; set; }
            public string StarSystem { get; set; }
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long? MarketID { get; set; }
            public string StorageSlot { get; set; }
            public int? TransferCost { get; set; }
            public int? TransferTime { get; set; } // TODO: check data type
            public bool Hot { get; set; }
            public string EngineerModifications { get; set; }
            public int? Level { get; set; }
            public decimal? Quality { get; set; }
            public bool? InTransit { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public List<ModuleItem> Items { get; set; }
    }
}
