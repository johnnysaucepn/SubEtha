using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal
{
    [ExcludeFromCodeCoverage]
    [JournalName("Fileheader")]
    public class FileHeader : JournalEntryBase
    {
        [JournalName("part")]
        public int Part { get; set; }
        [JournalName("language")]
        public string Language { get; set; }
        [JournalName("gameversion")]
        public string GameVersion { get; set; }
        [JournalName("build")]
        public string Build { get; set; }
        public bool Odyssey { get; set; }
    }
}
