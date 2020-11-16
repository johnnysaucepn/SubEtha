using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class MaterialTrade : JournalEntryBase
    {
        public class MaterialItem
        {
            public string Material { get; set; } // Note: name
            public string Category { get; set; } // TODO: check data type - enum? Encoded, Raw, etc?
            public int Quantity { get; set; }
        }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string TraderType { get; set; } // TODO: enum?
        public MaterialItem Paid { get; set; }
        public MaterialItem Received { get; set; }
    }
}
