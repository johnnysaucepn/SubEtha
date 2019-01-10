using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    // no sample
    [JournalName("FSDTarget")]
    public class FsdTarget : JournalEntryBase
    {
        public string StarSystem { get; set; }
        public string Name { get; set; } // TODO: check if this is the body
    }
}
