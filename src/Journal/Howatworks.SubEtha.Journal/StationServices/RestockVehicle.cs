using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class RestockVehicle : JournalEntryBase
    {
        public string Type { get; set; } // TODO: enum: SRV or fighter model
        public string Loadout { get; set; }
        public long Cost { get; set; }
        public int Count { get; set; }
    }
}
