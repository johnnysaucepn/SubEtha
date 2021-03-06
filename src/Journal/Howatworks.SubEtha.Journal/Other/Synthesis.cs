﻿using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.Other
{
    public class Synthesis : JournalEntryBase
    {
        public class MaterialItem
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }

        public string Name { get; set; }
        public List<MaterialItem> Materials { get; set; }
    }
}
