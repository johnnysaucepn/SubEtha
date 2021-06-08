using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Odyssey
{
    [ExcludeFromCodeCoverage]
    public class DropShipDeploy : JournalEntryBase // TODO: check this, including spelling of DropShip
    {
        public string StarSystem { get; set; } // NOTE: star system name
        public long SystemAddress { get; set; }
        public string Body { get; set; } // NOTE: body name
        [JournalName("BodyID")]
        public long BodyId { get; set; }
        public bool OnStation { get; set; }
        public bool OnPlanet { get; set; }
    }
}