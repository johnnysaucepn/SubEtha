﻿using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    [ExcludeFromCodeCoverage]
    public class RepairDrone
    {
        public decimal HullRepaired { get; set; } // TODO: check data type
        public decimal CockpitRepaired { get; set; } // TODO: check data type
        public decimal CorrosionRepaired { get; set; } // TODO: check data type
    }
}
