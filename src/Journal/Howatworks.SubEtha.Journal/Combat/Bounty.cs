﻿using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.Combat
{
    public class Bounty : JournalEntryBase
    {
        public class RewardItem
        {
            public string Faction { get; set; }
            public long Reward { get; set; }
        }

        public List<RewardItem> Rewards { get; set; }
        public string Target { get; set; }
        public string VictimFaction { get; set; }
        public long? TotalReward { get; set; }
        public int? SharedWithOthers { get; set; } // Note: number of players
        public string Faction { get; set; } // Note: skimmers only
        public long? Reward { get; set; } // Note: skimmers only
    }
}
