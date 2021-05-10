using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Powerplay
{
    [ExcludeFromCodeCoverage]
    public class PowerplayCollect : JournalEntryBase
    {
        public string Power { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
    }
}
