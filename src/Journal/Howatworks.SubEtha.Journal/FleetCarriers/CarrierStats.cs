using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierStats : JournalEntryBase
    {
        public class SpaceUsageItem
        {
            // TODO: check data types for all of these
            public int TotalCapacity { get; set; }
            public int Crew { get; set; }
            public int Cargo { get; set; }
            public int CargoSpaceReserved { get; set; }
            public int ShipPacks { get; set; }
            public int ModulePacks { get; set; }
            public int FreeSpace { get; set; }
        }
        public class FinanceItem
        {
            public long CarrierBalance { get; set; }
            public long ReserveBalance { get; set; }
            public long AvailableBalance { get; set; }
            public decimal ReservePercent { get; set; } // TODO: check data type
            public decimal TaxRate { get; set; } // TODO: check data type
        }

        public class CrewItem
        {
            public string CrewRole { get; set; } // TODO: enum?
            public bool Activated { get; set; }
            public bool Enabled { get; set; }
            public string CrewName { get; set; }
        }

        public class ShipPackItem
        {
            public string PackTheme { get; set; }
            public int PackTier { get; set; }
        }

        public class ModulePackItem
        {
            public string PackTheme { get; set; }
            public int PackTier { get; set; }
        }

        public long CarrierID { get; set; }
        public string Callsign { get; set; }
        public string Name { get; set; }
        public string DockingAccess { get; set; } // TODO: enum - all/none/friends/squadron/squadronfriends
        public bool AllowNotorious { get; set; }
        public int FuelLevel { get; set; } // TODO: check if this should be decimal for consistency?
        public decimal JumpRangeCurr { get; set; } // TODO: check type - docs say float
        public decimal JumpRangeMax { get; set; } // TODO: check type - docs say float
        public bool PendingDecommission { get; set; }
        public SpaceUsageItem SpaceUsage { get; set; }
        public FinanceItem Finance { get; set; }

        public List<CrewItem> Crew { get; set; }
        public List<ShipPackItem> ShipPacks { get; set; }
        public List<ModulePackItem> ModulePacks { get; set; }
    }
}
