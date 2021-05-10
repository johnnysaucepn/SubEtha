using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class ScientificResearch : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Name { get; set; } // Note: material name TODO: localised?
        public string Category { get; set; } // TODO: localised?
        public int Count { get; set; }
    }
}
