using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class Outfitting : JournalEntryBase
    {
        public class OutfittingItem
        {
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "As per Journal documentation")]
            public long id { get; set; }
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
