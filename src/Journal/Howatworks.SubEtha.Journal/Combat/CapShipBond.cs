using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Combat
{
    // TODO: no sample
    [ExcludeFromCodeCoverage]
    public class CapShipBond : JournalEntryBase
    {
        public long Reward { get; set; }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
    }
}
