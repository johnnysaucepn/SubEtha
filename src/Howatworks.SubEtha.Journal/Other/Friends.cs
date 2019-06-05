namespace Howatworks.SubEtha.Journal.Other
{
    public class Friends : JournalEntryBase
    {
        public string Status { get; set; } // TODO: enum? Requested, Declined, Added, Lost, Offline, Online
        public string Name { get; set; }
    }
}
