﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Trade
{
    [ExcludeFromCodeCoverage]
    public class MiningRefined : JournalEntryBase
    {
        public string Type { get; set; }
        public string Type_Localised { get; set; }
    }
}
