﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    // Note: no sample
    [ExcludeFromCodeCoverage]
    [Obsolete]
    public class EngineerLegacyConvert : JournalEntryBase
    {
        public class ModifierItem
        {
            // TODO: the docs suggest that one of these values should be an enum: WeaponMode, DamageType, CabinClass
            public string Label { get; set; }
            public decimal Value { get; set; }
            public decimal OriginalValue { get; set; }
            public bool LessIsGood { get; set; } // TODO: check data type - sample suggests 0/1?
            public string ValueStr { get; set; }
        }

        public string Engineer { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long EngineerID { get; set; } // TODO: check datatype
        public string Blueprint { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long BlueprintID { get; set; } // TODO: check datatype
        public int Level { get; set; }
        public decimal Quality { get; set; }
        public string ApplyExperimentalEffect { get; set; } // TODO: check data type
        public List<ModifierItem> Modifiers { get; set; }
        public bool IsPreview { get; set; }
    }
}
