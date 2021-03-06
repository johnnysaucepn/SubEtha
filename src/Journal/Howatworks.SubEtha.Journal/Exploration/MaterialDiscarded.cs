﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Exploration
{
    [ExcludeFromCodeCoverage]
    public class MaterialDiscarded : JournalEntryBase
    {
        public string Category { get; set; } // TODO: enum? Raw/Encoded/Manufactured
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
