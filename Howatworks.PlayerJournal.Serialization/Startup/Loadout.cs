using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Startup
{
    // NOTE: no sample data
    public class Loadout : JournalEntryBase
    {
        public class ModuleItem
        {
            public string Slot { get; set; } // NOTE: slot name
            public string Item { get; set; } // NOTE: item name
            public bool On { get; set; }
            public int Priority { get; set; } // TODO: check datatype
            public int Health { get; set; }// TODO: check datatype
            public long Value { get; set; }// TODO: check datatype
            public int? AmmoInClip { get; set; }// TODO: check datatype
            public int? AmmoInHopper { get; set; }// TODO: check datatype

            public ModuleItemEngineering Engineering { get; set; }
        }

        public class ModuleItemEngineering
        {
            public string EngineerId { get; set; } // TODO: check datatype
            public string Engineer { get; set; } // NOTE: name
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public string BlueprintID { get; set; } // TODO: check datatype
            public string BlueprintName { get; set; }
            public int Level { get; set; } // TODO: check datatype
            public decimal Quality { get; set; }  // TODO: check datatype
            public string ExperimentalEffect { get; set; } // Note: name

            public List<ModuleItemEngineeringModificationItem> Modifications { get; set; }
        }

        public class ModuleItemEngineeringModificationItem
        {
            public string Label { get; set; } // NOTE: see 13.11
            public string Value { get; set; } // TODO: check datatype
            public string OriginalValue { get; set; } // TODO: check datatype
            public bool LessIsGood { get; set; }
        }

        public string Ship { get; set; } // NOTE: Ship type
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
        public string ShipName { get; set; }
        public string ShipIdent { get; set; }
        public long? HullValue { get; set; } // TODO: check datatype
        public long? ModulesValue { get; set; } // TODO: check datatype
        public int HullHealth { get; set; } // TODO: check datatype
        public long Rebuy { get; set; } // TODO: check datatype
        public string Hot { get; set; } // TODO: check datatype

        public List<ModuleItem> Modules { get; set; }
    }
}
