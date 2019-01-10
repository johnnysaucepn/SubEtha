using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    public class LeaveBody : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; } // TODO: check data type
        public string Body { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string BodyID { get; set; } // TODO: check data type
    }
}
