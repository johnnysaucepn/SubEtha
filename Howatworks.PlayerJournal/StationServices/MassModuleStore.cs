using System.Collections.Generic;

namespace Howatworks.PlayerJournal.StationServices
{
    public class MassModuleStore : JournalEntryBase
    {
        public string Ship { get; set; }
        public int ShipID { get; set; }
        public List<ModuleItem> Items { get; set; }

        public class ModuleItem
        {
            public string Slot { get; set; }
            public string Name { get; set; }
            // TODO: is this localised?
            public string Name_Localised { get; set; }
            public string EngineerModifications { get; set; }
        }
    }

    
}
