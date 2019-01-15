using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Trade
{
    public class CollectCargo : JournalEntryBase
    {
        public string Type { get; set; }
        public string Type_Localised { get; set; }
        public bool Stolen { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long? MissionID { get; set; }
    }
}
