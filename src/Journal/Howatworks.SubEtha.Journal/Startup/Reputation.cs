using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Startup
{
    // no sample
    [ExcludeFromCodeCoverage]
    public class Reputation : JournalEntryBase
    {
        public decimal Empire { get; set; } // TODO: check data type
        public decimal Federation { get; set; } // TODO: check data type
        public decimal Independent { get; set; } // TODO: check data type
        public decimal Alliance { get; set; } // TODO: check data type
    }
}
