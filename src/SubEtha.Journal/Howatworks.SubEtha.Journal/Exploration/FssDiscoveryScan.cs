using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Exploration
{
    [ExcludeFromCodeCoverage]
    // Note: no sample
    [JournalName("FSSDiscoveryScan")]
    public class FssDiscoveryScan : JournalEntryBase
    {
        public decimal Progress { get; set; }
        public int BodyCount { get; set; }
        public int NonBodyCount { get; set; }
        public string SystemName { get; set; } // WARN: undocumented
        public long SystemAddress { get; set; } // WARN: undocumented
    }
}
