using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Startup
{
    // NOTE: no sample data
    public class Loadout : JournalEntryBase
    {
        public class ModuleItem
        {
            public string Slot { get; set; } // NOTE: slot name
            public string Item { get; set; } // NOTE: item name
            public bool On { get; set; }
            public int Priority { get; set; }
            public decimal Health { get; set; }
            public long? Value { get; set; } // NOTE: appears optional in 3.3
            public int? AmmoInClip { get; set; }
            public int? AmmoInHopper { get; set; }

            public ModuleItemEngineering Engineering { get; set; }
        }

        public class ModuleItemEngineering
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long EngineerID { get; set; }
            public string Engineer { get; set; } // NOTE: name
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long BlueprintID { get; set; }
            public string BlueprintName { get; set; }
            public int Level { get; set; }
            public decimal Quality { get; set; }
            public string ExperimentalEffect { get; set; } // Note: name
            public string ExperimentalEffect_Localised { get; set; }

            public List<ModuleItemEngineeringModifierItem> Modifiers { get; set; } // WARNING: docs say Modifications
        }

        public class ModuleItemEngineeringModifierItem
        {
            public string Label { get; set; } // NOTE: see 13.11
            public decimal Value { get; set; }
            public decimal OriginalValue { get; set; }
            public int LessIsGood { get; set; } // NOTE: not bool, 0 or 1
        }

        public class FuelCapacityItem
        {
            public decimal Main { get; set; } // TODO: check type
            public decimal Reserve { get; set; } // TODO: check type
        }

        public string Ship { get; set; } // NOTE: Ship type
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
        public string ShipName { get; set; }
        public string ShipIdent { get; set; }
        public long? HullValue { get; set; }
        public long? ModulesValue { get; set; }
        public decimal HullHealth { get; set; }
        public decimal UnladenMass { get; set; } // TODO: check type
        public FuelCapacityItem FuelCapacity { get; set; }
        public int CargoCapacity { get; set; } // TODO: check type
        public decimal MaxJumpRange { get; set; } // TODO: check type
        public long Rebuy { get; set; }
        public bool Hot { get; set; }

        public List<ModuleItem> Modules { get; set; }
    }
}
