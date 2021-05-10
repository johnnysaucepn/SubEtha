using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Exploration
{
    [ExcludeFromCodeCoverage]
    // Note: no sample
    [JournalName("FSSSignalDiscovered")]
    public class FssSignalDiscovered : JournalEntryBase
    {
        public string SignalName { get; set; }
        public string SignalName_Localised { get; set; }
        public string SpawningState { get; set; } // TODO: check data type - enum?
        public string SpawningState_Localised { get; set; }
        public string SpawningFaction { get; set; }
        public string SpawningFaction_Localised { get; set; }
        public decimal? TimeRemaining { get; set; } // TODO: in seconds
        public long SystemAddress { get; set; }
        public int? ThreatLevel { get; set; }
        public string USSType { get; set; } // TODO: check data type - enum? See USSDrop
        public string USSType_Localised { get; set; }
        public bool? IsStation { get; set; }
    }
}
