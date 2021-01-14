using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierJump : JournalEntryBase
    {
        public class StationFactionItem
        {
            public string Name { get; set; } // TODO: enum?
        }

        public class StationEconomyItem
        {
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public decimal Proportion { get; set; }
        }

        public class SystemFactionItem
        {
            public string Name { get; set; }
        }

        public bool Docked { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; } // TODO: enum?
        public long MarketID { get; set; }
        public StationFactionItem StationFaction { get; set; }
        public string StationGovernment { get; set; }
        public string StationGovernment_Localised { get; set; }
        public List<string> StationServices { get; set; } // TODO: enum? See Docked
        public string StationEconomy { get; set; }
        public string StationEconomy_Localised { get; set; }
        public List<StationEconomyItem> StationEconomies { get; set; }
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public List<decimal> StarPos { get; set; }
        public string SystemAllegiance { get; set; } // TODO: enum?
        public string SystemEconomy { get; set; }
        public string SystemEconomy_Localised { get; set; }
        public string SystemSecondEconomy { get; set; }
        public string SystemSecondEconomy_Localised { get; set; }
        public string SystemGovernment { get; set; }
        public string SystemGovernment_Localised { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemSecurity_Localised { get; set; }
        public long Population { get; set; }
        public string Body { get; set; } // NOTE: name
        public int BodyID { get; set; }
        public string BodyType { get; set; } // TODO: enum?
        public SystemFactionItem SystemFaction { get; set; }
    }
}
