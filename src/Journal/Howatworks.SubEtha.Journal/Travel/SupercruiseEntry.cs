using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    [ExcludeFromCodeCoverage]
    public class SupercruiseEntry : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; } // WARNING: undocumented
    }
}
