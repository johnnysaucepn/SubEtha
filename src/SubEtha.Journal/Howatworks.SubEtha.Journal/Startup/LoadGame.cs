using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Startup
{
    public class LoadGame : JournalEntryBase
    {
        public string Commander { get; set; } // NOTE: Commander name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string FID { get; set; }     // NOTE: Player ID
        public bool Horizons { get; set; }
        public string Ship { get; set; } // NOTE: ship type
        public string Ship_Localised { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int ShipID { get; set; }
        public bool? StartLanded { get; set; }
        public bool? StartDead { get; set; }
        public string GameMode { get; set; } // NOTE: Open, Solo or Group - consider enum?
        public string Group { get; set; }
        public long Credits { get; set; }
        public long Loan { get; set; }
        public string ShipName { get; set; } // NOTE: user-defined ship name
        public string ShipIdent { get; set; } // NOTE: user-defined ID string
        public decimal FuelLevel { get; set; }
        public decimal FuelCapacity { get; set; }

    }
}
