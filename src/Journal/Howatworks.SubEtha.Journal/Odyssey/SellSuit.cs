using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class SellSuit : JournalEntryBase
    {
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public long Price { get; set; }
        [JournalName("SuitID")]
        public long SuitId { get; set; } // WARN: not in doc samples
    }
}