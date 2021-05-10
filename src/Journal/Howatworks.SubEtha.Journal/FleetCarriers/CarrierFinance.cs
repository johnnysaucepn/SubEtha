using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierFinance : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public decimal TaxRate { get; set; } // TODO: check data type
        public long CarrierBalance { get; set; }
        public long ReserveBalance { get; set; }
        public long AvailableBalance { get; set; }
        public decimal ReservePercent { get; set; } // TODO: check data type
    }
}
