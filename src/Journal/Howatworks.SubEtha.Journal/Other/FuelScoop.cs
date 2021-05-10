using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class FuelScoop : JournalEntryBase
    {
        public decimal Scooped { get; set; }
        public decimal Total { get; set; }
    }
}
