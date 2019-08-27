using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class MissionAbandoned : JournalEntryBase
    {
        // TODO: is this localised?
        public string Name { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MissionID { get; set; }
        public long? Fine { get; set; }
    }
}
