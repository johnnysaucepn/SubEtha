using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Cargo : JournalEntryBase
    {
        public class CargoItem
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public int Stolen { get; set; }
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public int MissionID { get; set; }
        }

        public string Vessel { get; set; }
        public CargoItem[] Inventory { get; set; }
    }
}
