using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    // Note: no sample
    public class CommunityGoalDiscard : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long CGID { get; set; } // TODO: check data type
        public string Name { get; set; }
        public string System { get; set; }
    }
}
