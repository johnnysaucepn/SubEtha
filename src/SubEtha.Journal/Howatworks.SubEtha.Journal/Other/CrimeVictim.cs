using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class CrimeVictim : JournalEntryBase
    {
        public string Offender { get; set; } // TODO: check type
        public string Offender_Localised { get; set; }
        public string CrimeType { get; set; } // TODO: check enum
        public long? Fine { get; set; }
        public long? Bounty { get; set; }
    }
}
