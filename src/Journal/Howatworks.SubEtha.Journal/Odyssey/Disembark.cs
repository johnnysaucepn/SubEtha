using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class Disembark : JournalEntryBase
    {
        [JournalName("SRV")]
        public bool Srv { get; set; }
        public bool Taxi { get; set; }
        public bool Multicrew { get; set; }
        [JournalName("ID")]
        public int? Id { get; set; } // NOTE: player ship ID
        public string StarSystem { get; set; } // NOTE: star system name
        public long SystemAddress { get; set; }
        public string Body { get; set; } // NOTE: body name
        [JournalName("BodyID")]
        public long BodyId { get; set; }
        public bool OnStation { get; set; }
        public bool OnPlanet { get; set; }

        // If at a station
        public string StationName { get; set; }
        public string StationType { get; set; } // TODO: enum?
        [JournalName("MarketID")]
        public long? MarketId { get; set; }
    }
}