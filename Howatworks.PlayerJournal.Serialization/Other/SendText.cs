namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class SendText : JournalEntryBase
    {
        public string To { get; set; }
        public string Message { get; set; }
    }
}
