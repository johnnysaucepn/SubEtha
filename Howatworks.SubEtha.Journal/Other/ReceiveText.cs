namespace Howatworks.SubEtha.Journal.Other
{
    public class ReceiveText : JournalEntryBase
    {
        public string From { get; set; }
        public string From_Localised { get; set; }
        public string Message { get; set; }
        public string Message_Localised { get; set; }
        public string Channel { get; set; } // TODO: enum? (wing/local/voicechat/friend/player/npc/squadron/starsystem)
    }
}
