using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Howatworks.SubEtha.Journal.StationServices
{
    [ExcludeFromCodeCoverage]
    public class Repair : JournalEntryBase
    {
        public class RepairItem
        {
            // TODO: check if localised?
            public string Item { get; set; }
        }

        #region Ship
        // TODO: check if localised?
        public string Item { get; set; } // TODO: enum: all, wear, hull, paint, or name of module

        #endregion

        #region Fleet Carrier
        public List<RepairItem> Items { get; set; }
        #endregion

        public long Cost { get; set; }
    }
}
