using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Exploration
{
    [ExcludeFromCodeCoverage]
    // Note: no sample
    public class DiscoveryScan : JournalEntryBase
    {
        public long SystemAddress { get; set; }
        public int Bodies { get; set; }
    }
}
