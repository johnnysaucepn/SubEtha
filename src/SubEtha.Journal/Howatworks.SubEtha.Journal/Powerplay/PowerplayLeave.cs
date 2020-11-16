using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Powerplay
{
    [ExcludeFromCodeCoverage]
    public class PowerplayLeave : JournalEntryBase
    {
        public string Power { get; set; }
    }
}
