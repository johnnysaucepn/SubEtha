using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    [ExcludeFromCodeCoverage]
    public class CarrierDepositFuel : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public int Amount { get; set; } // TODO: check if this should be decimal
        public int Total { get; set; } // TODO: check if this should be decimal
    }
}
