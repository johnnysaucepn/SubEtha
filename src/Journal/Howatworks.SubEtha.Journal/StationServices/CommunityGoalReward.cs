using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    // TODO: Obtain sample of CommunityGoalReward
    [ExcludeFromCodeCoverage]
    public class CommunityGoalReward : JournalEntryBase
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long CGID { get; set; } // TODO: check data type
        public string Name { get; set; }
        public string System { get; set; }
        public long Reward { get; set; }
    }
}
