using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class SellWeapon : JournalEntryBase
    {
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public long Price { get; set; }
        [JournalName("SuitModuleID")]
        public long SuitModuleId { get; set; } // WARN: not in doc samples
    }
}