using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class LoadoutRemoveModule : JournalEntryBase
    {
        [JournalName("SuitID")]
        public long SuitId { get; set; } // TODO: check this
        public string SuitName { get; set; } // TODO: check this - localised?
        public string SlotName { get; set; }
        [JournalName("LoadoutID")]
        public long LoadoutId { get; set; }
        public string LoadoutName { get; set; }
        public string ModuleName { get; set; }
        public string ModuleName_Localised { get; set; }
        [JournalName("SuitModuleID")]
        public long SuitModuleId { get; set; } // TODO: check this
    }
}