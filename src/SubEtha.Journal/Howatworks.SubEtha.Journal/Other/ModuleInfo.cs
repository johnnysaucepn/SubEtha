using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.Other
{
    // TODO: inferred from sample only, not documented explicitly
    // Note: written to ModulesInfo.json
    public class ModuleInfo : JournalEntryBase
    {
        public class ModuleItem
        {
            public string Slot { get; set; } // TODO: enum?
            public string Item { get; set; }
            public decimal Power { get; set; }
            public int? Priority { get; set; }
        }

        public List<ModuleItem> Modules { get; set; }
    }
}
