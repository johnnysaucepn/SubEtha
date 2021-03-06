﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Exploration
{
    [ExcludeFromCodeCoverage]
    public class MaterialDiscovered : JournalEntryBase
    {
        public string Category { get; set; } // TODO: enum? Raw/Encoded/Manufactured
        public string Name { get; set; }
        public int DiscoveryNumber { get; set; }
    }
}
