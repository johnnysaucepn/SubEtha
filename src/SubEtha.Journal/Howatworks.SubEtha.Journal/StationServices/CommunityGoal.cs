using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class CommunityGoal : JournalEntryBase
    {
        public class GoalItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long CGID { get; set; } // TODO: check data type
            public string Title { get; set; }
            public string SystemName { get; set; }
            public string MarketName { get; set; }
            public DateTimeOffset Expiry { get; set; } // TODO: check this parses correctly
            public bool IsComplete { get; set; }
            public int CurrentTotal { get; set; }
            public int PlayerContribution { get; set; }
            public int NumContributors { get; set; }
            public int PlayerPercentileBand { get; set; } // TODO: check data type
            public TopTierItem TopTier { get; set; } // TODO: not in sample
            public int? TopRankSize { get; set; }
            public bool? PlayerInTopRank { get; set; }
            public string TierReached { get; set; }
            public long Bonus { get; set; }
        }

        public class TopTierItem
        {
            public string Name { get; set; }
            public string Bonus { get; set; } // TODO: check data type
        }

        public List<GoalItem> CurrentGoals { get; set; }
    }
}
