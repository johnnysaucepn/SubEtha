using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.Travel
{
    [JournalName("FSDJump")]
    public class FsdJump : JournalEntryBase
    {
        public class FactionItem
        {
            public string Name { get; set; }
            public string FactionState { get; set; } // TODO: consider enum?
            public string Government { get; set; } // TODO: consider enum?
            public decimal Influence { get; set; }
            public string Allegiance { get; set; } // WARNING: not in docs // TODO: enum?
            public string Happiness { get; set; } // TODO: consider enum - (Elated, Happy, Discontented, Unhappy, Despondent)
            public string Happiness_Localised { get; set; }
            public decimal MyReputation { get; set; }
            public List<FactionItemStateItem> PendingStates { get; set; }
            public List<FactionItemStateItem> RecoveringStates { get; set; } // TODO: check spelling?
            public List<FactionItemStateItem> ActiveStates { get; set; } // TODO: No trend value, check this is okay
            public bool? SquadronFaction { get; set; }
            public bool? HappiestSystem { get; set; }
            public bool? HomeSystem { get; set; }
        }

        public class FactionItemStateItem
        {
            public string State { get; set; } // TODO: enum?
            public decimal Trend { get; set; } // TODO: check data type
        }

        public class SystemFactionItem
        {
            public string Name { get; set; }
            public string FactionState { get; set; } // TODO: consider enum?
        }

        public class ConflictItem
        {
            public string WarType { get; set; } // TODO: consider enum?
            public string Status { get; set; } // TODO: check data type?
            public ConflictItemFactionItem Faction1 { get; set; }
            public ConflictItemFactionItem Faction2 { get; set; }
        }

        public class ConflictItemFactionItem
        {
            public string Name { get; set; }
            public string Stake { get; set; } // TODO: check data type?
            public int WonDays { get; set; } // TODO: check data type?
        }

        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public List<decimal> StarPos { get; set; }
        // TODO: Report that Body is never populated
        public string Body { get; set; } // Note: name
        public decimal JumpDist { get; set; }
        public decimal FuelUsed { get; set; }
        public decimal FuelLevel { get; set; }
        public bool BoostUsed { get; set; }
        public SystemFactionItem SystemFaction { get; set; }
        public string FactionState { get; set; }
        public string SystemAllegiance { get; set; }
        public string SystemEconomy { get; set; }
        public string SystemEconomy_Localised { get; set; }
        public string SystemSecondEconomy { get; set; }
        public string SystemSecondEconomy_Localised { get; set; }
        public string SystemGovernment { get; set; }
        public string SystemGovernment_Localised { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemSecurity_Localised { get; set; }
        public long Population { get; set; }
        public bool Wanted { get; set; } // TODO: check data type
        public List<FactionItem> Factions { get; set; }
        public List<ConflictItem> Conflicts { get; set; }
        public List<string> Powers { get; set; }
        // TODO: consider enum -  ("InPrepareRadius", "Prepared", "Exploited", "Contested", "Controlled", "Turmoil", "HomeSystem")
        public string PowerplayState { get; set; }

    }
}
