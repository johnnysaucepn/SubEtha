using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class CreateSuitLoadout : JournalEntryBase
    {
        public class ModuleItem
        {
            public string SlotName { get; set; }
            public string ModuleName { get; set; }
            public string ModuleName_Localised { get; set; }
            [JournalName("SuitModuleID")]
            public long SuitModuleId { get; set; }
        }

        [JournalName("SuitID")]
        public long SuitId { get; set; } // TODO: check this
        public string SuitName { get; set; } // TODO: check this - localised?
        [JournalName("LoadoutID")]
        public long LoadoutId { get; set; }
        public string LoadoutName { get; set; }
        public List<ModuleItem> Modules { get; set; }
    }
}