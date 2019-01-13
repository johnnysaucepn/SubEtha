using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class SearchAndRescue : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Name { get; set; } // Note: material name TODO: localised?
        public int Count { get; set; }
        public int Reward { get; set; }

    }
}
