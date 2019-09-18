using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.SubEtha.Journal.Exploration
{

    [JournalName("SAASignalsFound")]
    public class SaaSignalsFound : JournalEntryBase
    {
        public class SignalItem
        {
            public string Type { get; set; }
            public string Type_Localised { get; set; }
            public int Count { get; set; }
        }

        public long SystemAddress { get; set; }
        public string BodyName { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; }
        public List<SignalItem> Signals { get; set; }
    }

}
