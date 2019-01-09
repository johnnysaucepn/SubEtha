using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Startup
{
    // no sample
    public class Reputation : JournalEntryBase
    {
        public decimal Empire { get; set; } // TODO: check data type
        public decimal Federation { get; set; } // TODO: check data type
        public decimal Independent { get; set; } // TODO: check data type
        public decimal Alliance{ get; set; } // TODO: check data type
    }
}
