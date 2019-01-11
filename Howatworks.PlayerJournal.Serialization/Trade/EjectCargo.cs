using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Trade
{
    public class EjectCargo : JournalEntryBase
    {
        public string Type { get; set; }
        public int Count { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long? MissionID { get; set; }
        public bool Abandoned { get; set; }
        public string PowerplayOrigin { get; set; }
    }
}
