using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    [ExcludeFromCodeCoverage]
    public class StartJump : JournalEntryBase
    {
        public string JumpType { get; set; } // TODO: consider enum - Hyperspace, Supercruise
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public string StarClass { get; set; } // TODO: check data type
    }
}
