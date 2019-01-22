using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class SearchAndRescue : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string Name { get; set; } // Note: material name
        public string Name_Localised { get; set; }
        public int Count { get; set; }
        public long Reward { get; set; }

    }
}
