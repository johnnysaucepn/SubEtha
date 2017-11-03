namespace Howatworks.PlayerJournal.Other
{
    public class ReceiveText : JournalEntryBase
    {
        public string From { get; set; }
        public string Message { get; set; }
        // TODO: enumerate this?
        public string Channel { get; set; }
    }
}
