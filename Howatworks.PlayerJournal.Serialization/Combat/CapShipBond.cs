﻿namespace Howatworks.PlayerJournal.Serialization.Combat
{
    // TODO: no sample
    public class CapShipBond : JournalEntryBase
    {
        public long Reward { get; set; }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
    }
}
