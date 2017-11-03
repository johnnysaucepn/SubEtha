namespace Howatworks.PlayerJournal.Other
{
    public class DatalinkVoucher : JournalEntryBase
    {
        public int Reward { get; set; }
        // TODO: check these
        public string VictimFaction { get; set; }
        public string PayeeFaction { get; set; }

    }
}
