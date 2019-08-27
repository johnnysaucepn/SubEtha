namespace Howatworks.SubEtha.Journal.Other
{
    public class DatalinkVoucher : JournalEntryBase
    {
        public long Reward { get; set; }
        public string VictimFaction { get; set; }
        public string PayeeFaction { get; set; }

    }
}
