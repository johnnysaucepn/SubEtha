using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    // Note: no sample
    [ExcludeFromCodeCoverage]
    public class CrewAssign : JournalEntryBase
    {
        public string Name { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long CrewID { get; set; } // TODO: check data type
        public string Role { get; set; }
    }
}
