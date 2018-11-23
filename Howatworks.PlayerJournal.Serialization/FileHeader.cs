namespace Howatworks.PlayerJournal.Serialization
{
    [JournalName("fileheader")]
    public class FileHeader : JournalEntryBase
    {
        public int Part { get; set; }
        public string Language { get; set; }
        public string GameVersion { get; set; }
        public string Build { get; set; }
    }
}
