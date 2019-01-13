using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class MissionCompleted : JournalEntryBase
    {
        public class CommodityRewardItem
        {
            // TODO: localised?
            public string Name { get; set; }
            public int Count { get; set; }
        }

        public class MaterialRewardItem
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int Count { get; set; }
        }

        public class FactionEffectItem
        {
            public string Faction { get; set; }
            public List<EffectItem> Effects { get; set; }
            public List<InfluenceItem> Influence { get; set; }
            public string Reputation { get; set; } // TODO: enum - UpGood, UpBad, DownGood, DownBad
        }

        public class EffectItem
        {
            public string Effect { get; set; }
            public string Trend { get; set; } // TODO: enum - UpGood, UpBad, DownGood, DownBad
        }

        public class InfluenceItem
        {
            public long SystemAddress { get; set; } // TODO: sample says SystemAddress, docs say System (name)
            public string Trend { get; set; } // TODO: enum - UpGood, UpBad, DownGood, DownBad
        }

        // TODO: localised?
        public string Name { get; set; }
        public string Faction { get; set; }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MissionID { get; set; }

        #region Optional

        public string Commodity { get; set; }
        public string Commodity_Localised { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public int? Reward { get; set; }
        public int? Donation { get; set; }
        public List<string> PermitsAwarded { get; set; } // TODO: check data type
        public List<CommodityRewardItem> CommodityReward { get; set; }
        public List<MaterialRewardItem> MaterialsReward { get; set; }

        #endregion
    }
}
