using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Travel
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
            public decimal MyReputation { get; set; }
            public List<FactionItemStateItem> PendingStates { get; set; }
            public List<FactionItemStateItem> RecoveringStates { get; set; } // TODO: check spelling?
            public List<FactionItemStateItem> ActiveStates { get; set; } // TODO: No trend value, check this is okay
            public bool SquadronFaction { get; set; }
            public bool HappiestSystem { get; set; }
            public bool HomeSystem { get; set; }
        }

        public class FactionItemStateItem
        {
            public string State { get; set; } // TODO: check name
            public string Trend { get; set; } // TODO: check data type?
        }

        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public List<decimal> StarPos { get; set; }
        public string Body { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string BodyID { get; set; } // TODO: check data type
        public string BodyType { get; set; }
        public bool Docked { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; } // TODO: check data type
        public string SystemFaction { get; set; }
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
        public bool Wanted { get; set; } // TODO: check data type
        public List<FactionItem> Factions { get; set; }
        public List<string> Powers { get; set; }
        // TODO: consider enum -  ("InPrepareRadius", "Prepared", "Exploited", "Contested", "Controlled", "Turmoil", "HomeSystem")
        public string PowerplayState { get; set; }
    }
}
