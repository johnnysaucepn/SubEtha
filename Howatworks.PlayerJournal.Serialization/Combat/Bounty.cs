using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.Combat
{
    public class Bounty : JournalEntryBase
    {
        public class FactionRewardItem
        {
            public string Faction { get; set; }
            public int Reward { get; set; }
        }

        public List<FactionRewardItem> Rewards { get; set; }
        public string Target { get; set; } // TODO: UNDOCUMENTED - ship type
        public string VictimFaction { get; set; }
        public int TotalReward { get; set; }
        public int SharedWithOthers { get; set; }
        public string Faction { get; set; } // Note: skimmers only
        public int Reward { get; set; } // Note: skimmers only

    }
}
