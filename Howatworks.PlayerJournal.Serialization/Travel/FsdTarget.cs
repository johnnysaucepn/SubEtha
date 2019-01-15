using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Travel
{
    [JournalName("FSDTarget")]
    public class FsdTarget : JournalEntryBase
    {
        public long SystemAddress { get; set; }
        //public string StarSystem { get; set; }
        public string Name { get; set; }
    }
}
