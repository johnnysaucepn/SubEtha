using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    public class Docked : JournalEntryBase
    {
        public class StationEconomyItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public decimal Proportion { get; set; }
        }

        public class StationFactionItem
        {
            public string Name { get; set; }
            public string FactionState { get; set; } // TODO: consider enum?
        }

        public string StationName { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public long SystemAddress { get; set; }
        public string StationType { get; set; }
        public string StarSystem { get; set; }
        public bool CockpitBreach { get; set; }
        public StationFactionItem StationFaction { get; set; }
        public string StationAllegiance { get; set; } // TODO: enum?
        public string StationEconomy { get; set; } // TODO: enum?
        public string StationEconomy_Localised { get; set; }
        public List<StationEconomyItem> StationEconomies { get; set; }
        public string StationGovernment { get; set; } // TODO: enum?
        public string StationGovernment_Localised { get; set; }
        public decimal DistFromStarLS { get; set; }
        // TODO: consider enum - Dock, Autodock, BlackMarket, Commodities, Contacts, Exploration, Initiatives, Missions, Outfitting, CrewLounge, Rearm, Refuel, Repair, Shipyard, Tuning, Workshop, MissionsGenerated, Facilitator, Research, FlightController, StationOperations, OnDockMission, Powerplay, SearchAndRescue
        public List<string> StationServices { get; set; }
        public bool Wanted { get; set; } // TODO: check data type
        public bool ActiveFine { get; set; }
    }
}
