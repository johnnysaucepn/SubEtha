using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Startup
{
    public class Statistics : JournalEntryBase
    {
        public class BankAccountItem
        {
            public long Current_Wealth { get; set; }
            public long Spent_On_Ships { get; set; }
            public long Spent_On_Outfitting { get; set; }
            public long Spent_On_Repairs { get; set; }
            public long Spent_On_Fuel { get; set; }
            public long Spent_On_Ammo_Consumables { get; set; }
            public int Insurance_Claims { get; set; }
            public long Spent_On_Insurance { get; set; }
            public int Owned_Ship_Count { get; set; } // WARNING: undocumented
        }

        public class CombatItem
        {
            public int Bounties_Claimed { get; set; }
            public long Bounty_Hunting_Profit { get; set; }
            public int Combat_Bonds { get; set; }
            public long Combat_Bond_Profits { get; set; }
            public int Assassinations { get; set; }
            public long Assassination_Profits { get; set; }
            public long Highest_Single_Reward { get; set; }
            public int Skimmers_Killed { get; set; }
        }

        public class CrimeItem
        {
            public int Notoriety { get; set; } // WARNING: undocumented
            public int Fines { get; set; }
            public long Total_Fines { get; set; }
            public int Bounties_Received { get; set; }
            public long Total_Bounties { get; set; }
            public long Highest_Bounty { get; set; }
        }

        public class SmugglingItem
        {
            public int Black_Markets_Traded_With { get; set; }
            public long Black_Markets_Profits { get; set; }
            public int Resources_Smuggled { get; set; }
            public long Average_Profit { get; set; }
            public long Highest_Single_Transaction { get; set; }
        }

        public class TradingItem
        {
            public int Markets_Traded_With { get; set; }
            public long Market_Profits { get; set; }
            public int Resources_Traded { get; set; }
            public long Average_Profit { get; set; }
            public long Highest_Single_Transaction { get; set; }
        }

        public class MiningItem
        {
            public long Mining_Profits { get; set; }
            public int Quantity_Mined { get; set; }
            public int Materials_Collected { get; set; }
        }

        public class ExplorationItem
        {
            public int Systems_Visited { get; set; }
            public decimal? Fuel_Scooped { get; set; } // WARNING: not found
            public decimal? Fuel_Purchased { get; set; } // WARNING: not found
            public long Exploration_Profits { get; set; }
            public int Planets_Scanned_To_Level_2 { get; set; }
            public int Planets_Scanned_To_Level_3 { get; set; }
            public int Efficient_Scans { get; set; } // WARNING: undocumented
            public long Highest_Payout { get; set; }
            public int Total_Hyperspace_Distance { get; set; }
            public int Total_Hyperspace_Jumps { get; set; }
            public decimal Greatest_Distance_From_Start { get; set; }
            public int Time_Played { get; set; } // Note: time in seconds
        }

        public class PassengersItem
        {
            public int Passengers_Missions_Accepted { get; set; } // WARNING: undocumented
            public int Passengers_Missions_Disgruntled { get; set; } // WARNING: undocumented
            public int Passengers_Missions_Bulk { get; set; }
            public int Passengers_Missions_VIP { get; set; }
            public int Passengers_Missions_Delivered { get; set; }
            public int Passengers_Missions_Ejected { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class SearchAndRescueItem
        {
            public int SearchRescue_Traded { get; set; }
            public long SearchRescue_Profit { get; set; }
            public int SearchRescue_Count { get; set; }
        }

        public class CraftingItem
        {
            public long? Spent_On_Crafting { get; set; } // TODO: not found? perhaps need new data
            public int Count_Of_Used_Engineers { get; set; }
            public int Recipes_Generated { get; set; }
            public int Recipes_Generated_Rank_1 { get; set; }
            public int Recipes_Generated_Rank_2 { get; set; }
            public int Recipes_Generated_Rank_3 { get; set; }
            public int Recipes_Generated_Rank_4 { get; set; }
            public int Recipes_Generated_Rank_5 { get; set; }
            public int Recipes_Applied { get; set; } // TODO: not found? perhaps need new data
            public int Recipes_Applied_Rank_1 { get; set; } // TODO: not found? perhaps need new data
            public int Recipes_Applied_Rank_2 { get; set; } // TODO: not found? perhaps need new data
            public int Recipes_Applied_Rank_3 { get; set; } // TODO: not found? perhaps need new data
            public int Recipes_Applied_Rank_4 { get; set; } // TODO: not found? perhaps need new data
            public int Recipes_Applied_Rank_5 { get; set; } // TODO: not found? perhaps need new data
            public int Recipes_Applied_On_Previously_Modified_Modules { get; set; } // TODO: not found? perhaps need new data
        }

        public class CrewItem // TODO: may be empty, check how this serialises
        {
            public long NpcCrew_TotalWages { get; set; }
            public int NpcCrew_Hired { get; set; }
            public int NpcCrew_Fired { get; set; }
            public int NpcCrew_Died { get; set; }
        }

        public class MulticrewItem
        {
            public int Multicrew_Time_Total { get; set; } // Note: time in seconds
            public int Multicrew_Gunner_Time_Total { get; set; } // Note: time in seconds
            public int Multicrew_Fighter_Time_Total { get; set; } // Note: time in seconds
            public long Multicrew_Credits_Total { get; set; }
            public long Multicrew_Fines_Total { get; set; }
        }

        public class MaterialTraderStatsItem // WARNING: undocumented
        {
            public int Trades_Completed { get; set; }
            public int Materials_Traded { get; set; }
            public int Encoded_Materials_Traded { get; set; }
            public int Raw_Materials_Traded { get; set; }
            public int Grade_1_Materials_Traded { get; set; }
            public int Grade_2_Materials_Traded { get; set; }
            public int Grade_3_Materials_Traded { get; set; }
            public int Grade_4_Materials_Traded { get; set; }
            public int Grade_5_Materials_Traded { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class CQCItem // WARNING: undocumented
        {
            public long CQC_Credits_Earned { get; set; }
            public int CQC_Time_Played { get; set; }
            public decimal CQC_KD { get; set; }
            public int CQC_Kills { get; set; }
            public decimal CQC_WL { get; set; } // TODO: check data type?
        }

        public BankAccountItem Bank_Account { get; set; }
        public CombatItem Combat { get; set; }
        public CrimeItem Crime { get; set; }
        public SmugglingItem Smuggling { get; set; }
        public TradingItem Trading { get; set; }
        public MiningItem Mining { get; set; }
        public ExplorationItem Exploration { get; set; }
        public PassengersItem Passengers { get; set; }
        public SearchAndRescueItem Search_And_Rescue { get; set; }
        public CraftingItem Crafting { get; set; }
        public CrewItem Crew { get; set; }
        public MulticrewItem Multicrew { get; set; }
        public MaterialTraderStatsItem Material_Trader_Stats { get; set; } // WARNING: undocumented
        public CQCItem CQC { get; set; } // WARNING: undocumented

    }
}
