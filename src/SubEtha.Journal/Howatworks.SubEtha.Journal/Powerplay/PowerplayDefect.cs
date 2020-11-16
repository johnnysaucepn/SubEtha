using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Powerplay
{
    [ExcludeFromCodeCoverage]
    public class PowerplayDefect : JournalEntryBase
    {
        public string FromPower { get; set; }
        public string ToPower { get; set; }
    }
}
