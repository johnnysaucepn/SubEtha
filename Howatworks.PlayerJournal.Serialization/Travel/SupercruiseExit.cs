using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class SupercruiseExit : JournalEntryBase
    {
        public long SystemAddress { get; set; } // WARNING: not in docs
        public string StarSystem { get; set; }
        public string Body { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; }
        public string BodyType { get; set; }
    }
}
