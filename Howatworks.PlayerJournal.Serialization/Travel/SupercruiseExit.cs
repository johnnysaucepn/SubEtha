using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class SupercruiseExit : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public string Body { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; } // TODO: check data type
        public string BodyType { get; set; }
    }
}
