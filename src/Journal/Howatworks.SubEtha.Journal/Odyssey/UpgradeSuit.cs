using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class UpgradeSuit : JournalEntryBase
    {
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        [JournalName("SuitID")]
        public long SuitId { get; set; } // WARN: not in doc samples
        public int Class { get; set; }
        public long Cost { get; set; } // WARN: not Price
    }
}