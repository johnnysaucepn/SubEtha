using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    public class Location : JournalEntryBase
    {
        // TODO: described as 'similar' to FSDJump - check if they can be the same
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
            public string State { get; set; }
            public string Trend { get; set; } // TODO: check data type?
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
        public string Body { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; }

        public string BodyType { get; set; }
        public decimal? DistFromStarLS { get; set; }
        public bool Docked { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long? MarketID { get; set; }
        public SystemFactionItem SystemFaction { get; set; }
        public string FactionState { get; set; } // TODO: enum?
        public string SystemAllegiance { get; set; } // TODO: enum?
        public string SystemEconomy { get; set; }
        public string SystemEconomy_Localised { get; set; }
        public string SystemSecondEconomy { get; set; }
        public string SystemSecondEconomy_Localised { get; set; }
        public string SystemGovernment { get; set; }
        public string SystemGovernment_Localised { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemSecurity_Localised { get; set; }
        public long Population { get; set; } // WARNING: not in docs
        public bool Wanted { get; set; } // TODO: check data type
        public List<FactionItem> Factions { get; set; }
        public List<ConflictItem> Conflicts { get; set; }
        public List<string> Powers { get; set; }
        // TODO: consider enum -  ("InPrepareRadius", "Prepared", "Exploited", "Contested", "Controlled", "Turmoil", "HomeSystem")
        public string PowerplayState { get; set; }
    }
}
