using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class DatalinkVoucher : JournalEntryBase
    {
        public long Reward { get; set; }
        public string VictimFaction { get; set; }
        public string PayeeFaction { get; set; }

    }
}
