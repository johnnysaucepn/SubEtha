using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Startup
{
    public class Cargo : JournalEntryBase
    {
        public class CargoItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public int Count { get; set; }
            public int Stolen { get; set; } // Note: how many of these items are stolen?
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long? MissionID { get; set; }
        }

        public string Vessel { get; set; } // TODO: consider enum - Ship, SRV
        public int Count { get; set; } // WARNING: not in docs
        public List<CargoItem> Inventory { get; set; }
    }
}
