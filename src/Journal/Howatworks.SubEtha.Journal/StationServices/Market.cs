using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class Market : JournalEntryBase
    {
        public class MarketItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "As per Journal documentation")]
            public long id { get; set; }
            public string Name { get; set; }
            public string Name_Localised { get; set; }
            public string Category { get; set; }
            public string Category_Localised { get; set; }
            public long BuyPrice { get; set; }
            public long SellPrice { get; set; }
            public long MeanPrice { get; set; }
            public int StockBracket { get; set; }
            public int DemandBracket { get; set; }
            public int Stock { get; set; }
            public int Demand { get; set; }
            public bool Consumer { get; set; }
            public bool Producer { get; set; }
            public bool Rare { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; } // TODO: enum // NOTE: not documented
        public string StarSystem { get; set; } // Note: name

        #region Full market prices, used in market.json

        public List<MarketItem> Items { get; set; }

        #endregion
    }
}
