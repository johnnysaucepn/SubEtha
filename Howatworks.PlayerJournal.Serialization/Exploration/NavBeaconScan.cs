using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    public class NavBeaconScan : JournalEntryBase
    {
        public int NumBodies { get; set; }
        public long SystemAddress { get; set; }
    }
}
