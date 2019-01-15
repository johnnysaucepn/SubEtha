using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    // Note: no sample
    public class MassModuleStore : JournalEntryBase
    {
        public class ModuleItem
        {
            public string Slot { get; set; }

            public string Name { get; set; }

            // TODO: is this localised?
            public string Name_Localised { get; set; }
            public bool Hot { get; set; }
            public string EngineerModifications { get; set; } // TODO: check data type
            public int Level { get; set; } // TODO: check datatype
            public decimal Quality { get; set; }  // TODO: check datatype

        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Ship { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipId { get; set; } // TODO: check name
        public List<ModuleItem> Items { get; set; }
    }

}
