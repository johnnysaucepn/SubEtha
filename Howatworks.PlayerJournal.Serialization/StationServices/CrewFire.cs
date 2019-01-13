using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class CrewFire : JournalEntryBase
    {
        public string Name { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long CrewID { get; set; } // TODO: check data type
    }
}
