using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Powerplay
{
    [ExcludeFromCodeCoverage]
    public class PowerplaySalary : JournalEntryBase
    {
        public string Power { get; set; }
        public long Amount { get; set; }
    }
}
