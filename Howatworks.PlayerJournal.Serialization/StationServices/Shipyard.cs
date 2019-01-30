using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class Shipyard : JournalEntryBase
    {
        public class PriceItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "As per Journal documentation")]
            public long id { get; set; }

            public string ShipType { get; set; }
            public string ShipType_Localised { get; set; }
            public long ShipPrice { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string StationName { get; set; }
        public string StarSystem { get; set; }

        #region only for standalone file Shipyard.json

        public bool Horizons { get; set; }
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public bool AllowCobraMkIV { get; set; }
        [SuppressMessage("ReSharper", "IdentifierTypo")]
        public List<PriceItem> PriceList { get; set; } // WARNING: doc uses spelling Pricelist?

        #endregion
    }
}
