﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class Market : JournalEntryBase
    {
        public class MarketItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long id { get; set; } // TODO: check name
            public string Name { get; set; }
            public string Category { get; set; }
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
        public string StarSystem { get; set; } // Note: name

        #region Full market prices, used in market.json

        public List<MarketItem> Items { get; set; }

        #endregion
    }
}
