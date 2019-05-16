namespace Howatworks.SubEtha.Journal.Other
{
    public class CrimeVictim : JournalEntryBase
    {
        public string Offender { get; set; } // TODO: check type
        public string CrimeType { get; set; } // TODO: check enum
        public long? Fine { get; set; } // TODO: check type
        public long? Bounty { get; set; } // TODO: check type
    }
}
