using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class ApproachBody : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; } // TODO: check data type
        public string Body { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; } // TODO: check data type
    }
}
