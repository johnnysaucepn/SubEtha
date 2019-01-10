using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class MassModuleStore : JournalEntryBase
    {
        public class ModuleItem
        {
            public string Slot { get; set; }

            public string Name { get; set; }

            // TODO: is this localised?
            public string Name_Localised { get; set; }
            public string EngineerModifications { get; set; }
        }

        public string Ship { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
        public List<ModuleItem> Items { get; set; }
    }

}
