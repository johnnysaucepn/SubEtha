using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Powerplay
{
    [ExcludeFromCodeCoverage]
    public class PowerplayFastTrack : JournalEntryBase
    {
        public string Power { get; set; }
        public long Cost { get; set; }
    }
}
