using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    public class LeaveBody : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public string Body { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; }
    }
}
