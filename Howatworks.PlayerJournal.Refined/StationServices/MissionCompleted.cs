using System.Collections.Generic;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class MissionCompleted : JournalEntryBase
    {
        // TODO: localised?
        public string Name { get; set; }
        public string Faction { get; set; }
        public int MissionID { get; set; }

        #region Optional
        public string Commodity { get; set; }
        public string Commodity_Localised { get; set; }
        public int Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public int Reward { get; set; }
        public int Donation { get; set; }
        public List<string> PermitsAwarded { get; set; }
        public List<CommodityRewardItem> CommodityReward { get; set; }

        public class CommodityRewardItem
        {
            // TODO: localised?
            public string Name { get; set; }
            public int Count { get; set; }
        }

        #endregion
    }
}
