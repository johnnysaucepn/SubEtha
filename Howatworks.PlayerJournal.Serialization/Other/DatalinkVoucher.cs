﻿namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class DatalinkVoucher : JournalEntryBase
    {
        public long Reward { get; set; }
        // TODO: check these
        public string VictimFaction { get; set; }
        public string PayeeFaction { get; set; }

    }
}
