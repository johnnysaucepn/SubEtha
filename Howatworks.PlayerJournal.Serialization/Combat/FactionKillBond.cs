﻿namespace Howatworks.PlayerJournal.Serialization.Combat
{
    public class FactionKillBond : JournalEntryBase
    {
        public long Reward { get; set; }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
    }
}
