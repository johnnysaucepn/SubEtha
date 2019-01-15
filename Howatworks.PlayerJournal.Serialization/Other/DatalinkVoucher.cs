namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class DatalinkVoucher : JournalEntryBase
    {
        public long Reward { get; set; }
        public string VictimFaction { get; set; }
        public string PayeeFaction { get; set; }

    }
}
