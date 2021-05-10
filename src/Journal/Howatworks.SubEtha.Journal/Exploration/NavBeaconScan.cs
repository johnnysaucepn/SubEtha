using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Exploration
{
    [ExcludeFromCodeCoverage]
    public class NavBeaconScan : JournalEntryBase
    {
        public int NumBodies { get; set; }
        public long SystemAddress { get; set; }
    }
}
