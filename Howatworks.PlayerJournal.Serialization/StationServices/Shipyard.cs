using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class Shipyard : JournalEntryBase
    {
        public class PriceItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long id { get; set; } // TODO: check capitalisation

            public string ShipType { get; set; }
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
        public List<PriceItem> Pricelist { get; set; } // TODO: check capitalisation

        #endregion
    }
}
