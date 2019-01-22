using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class MissionCompleted : JournalEntryBase
    {
        public class CommodityRewardItem
        {
            public string Name { get; set; }// TODO: assuming localised
            public string Name_Localised { get; set; }
            public int Count { get; set; }
        }

        public class MaterialRewardItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public string Category { get; set; }
            public string Category_Localised { get; set; }
            public int Count { get; set; }
        }

        public class FactionEffectItem
        {
            public string Faction { get; set; } // NOTE: name
            public List<EffectItem> Effects { get; set; }
            public List<InfluenceItem> Influence { get; set; }
            public string ReputationTrend { get; set; } // WARNING: undocumented // TODO: enum - UpGood, UpBad, DownGood, DownBad 
            public string Reputation { get; set; } // TODO: enum - UpGood, UpBad, DownGood, DownBad
        }

        public class EffectItem
        {
            public string Effect { get; set; }
            public string Effect_Localised { get; set; }
            public string Trend { get; set; } // TODO: enum - UpGood, UpBad, DownGood, DownBad
        }

        public class InfluenceItem
        {
            public long SystemAddress { get; set; } // TODO: sample says SystemAddress, docs say System (name)
            public string Trend { get; set; } // TODO: enum - UpGood, UpBad, DownGood, DownBad
            public string Influence { get; set; } // TODO: +,+++,etc. // WARNING: undocumented
        }

        public string Name { get; set; } // WARNING: not localised
        public string Faction { get; set; } // NOTE: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MissionID { get; set; }

        #region Optional

        public string Commodity { get; set; }
        public string Commodity_Localised { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; } // NOTE: name
        public string DestinationSystem { get; set; } // NOTE: name, undocumented in event, described in changelog
        public string DestinationStation { get; set; } // NOTE: name, undocumented in event, described in changelog
        public long? Reward { get; set; }
        public long? Donation { get; set; }
        public List<string> PermitsAwarded { get; set; } // TODO: check data type
        public List<CommodityRewardItem> CommodityReward { get; set; }
        public List<MaterialRewardItem> MaterialsReward { get; set; }

        #endregion
    }
}
