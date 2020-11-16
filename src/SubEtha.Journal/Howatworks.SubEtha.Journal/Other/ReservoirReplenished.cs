using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class ReservoirReplenished : JournalEntryBase
    {
        public decimal FuelMain { get; set; }
        public decimal FuelReservoir { get; set; }
    }
}
