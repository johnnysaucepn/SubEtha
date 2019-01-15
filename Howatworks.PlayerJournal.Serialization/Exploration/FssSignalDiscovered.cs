using System;
using System.Collections.Generic;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.Exploration
{
    // Note: no sample
    [JournalName("FSSSignalDiscovered")]
    public class FssSignalDiscovered : JournalEntryBase
    {
        public string SignalName { get; set; }
        public string SignalName_Localised { get; set; }
        public string SpawningState { get; set; } // TODO: check data type - enum?
        public string SpawningFaction { get; set; }
        public int? TimeRemaining { get; set; } // TODO: check data type - in seconds
        public long SystemAddress { get; set; }
        public int? ThreatLevel { get; set; }
        public string USSType { get; set; } // TODO: check data type - enum? See USSDrop
        public string USSType_Localised { get; set; }
        public bool? IsStation { get; set; }
    }
}
