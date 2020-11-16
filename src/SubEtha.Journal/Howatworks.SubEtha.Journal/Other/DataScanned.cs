﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class DataScanned : JournalEntryBase
    {
        public string Type { get; set; } // TODO: enum?  "DataLink", "DataPoint", "ListeningPost", "AbandonedDataLog", "WreckedShip", etc
        public string Type_Localised { get; set; }
    }
}
