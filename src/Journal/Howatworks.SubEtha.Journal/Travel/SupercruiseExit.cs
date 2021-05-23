using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Travel
{
    [ExcludeFromCodeCoverage]
    public class SupercruiseExit : JournalEntryBase
    {
        public long SystemAddress { get; set; } // WARNING: not in docs
        public string StarSystem { get; set; }
        public string Body { get; set; } // Note: name
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; }
        public string BodyType { get; set; } // TODO: enum?

        // TODO: Are these still optional in Horizons?
        public bool? Taxi { get; set; }
        public bool? Multicrew { get; set; }
    }
}
