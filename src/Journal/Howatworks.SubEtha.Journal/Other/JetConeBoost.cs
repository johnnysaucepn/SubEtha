using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class JetConeBoost : JournalEntryBase
    {
        public decimal BoostValue { get; set; } // TODO: check data type
    }
}
