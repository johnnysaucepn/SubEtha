using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Other
{
    public class RepairDrone
    {
        public decimal HullRepaired { get; set; } // TODO: check data type
        public decimal CockpitRepaired { get; set; } // TODO: check data type
        public decimal CorrosionRepaired { get; set; } // TODO: check data type
    }
}
