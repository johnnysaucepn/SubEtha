using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.Other
{
    public class ApproachSettlement : JournalEntryBase
    {
        public string Name { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long SystemAddress { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int BodyID { get; set; }
        public string BodyName { get; set; }
    }
}
