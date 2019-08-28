using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class CrewFire : JournalEntryBase
    {
        public string Name { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long CrewID { get; set; } // TODO: check data type
    }
}
