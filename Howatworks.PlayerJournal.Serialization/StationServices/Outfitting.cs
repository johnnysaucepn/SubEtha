using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.PlayerJournal.Serialization.StationServices
{
    public class Outfitting : JournalEntryBase
    {
        public class OutfittingItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public long id { get; set; } // TODO: check capitalisation
            public string Name { get; set; }
            public long BuyPrice { get; set; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long MarketID { get; set; }

        public string StationName { get; set; }
        public string StarSystem { get; set; }

        #region For standalone file Outfitting.json

        public bool Horizons { get; set; }
        public List<OutfittingItem> Items { get; set; }

        #endregion
    }
}
