namespace Howatworks.PlayerJournal.Exploration
{
    public class Screenshot : JournalEntryBase
    {
        public string Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string System { get; set; }
        public string Body { get; set; }
    }
}
