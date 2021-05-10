using System.Collections.Generic;

namespace Howatworks.SubEtha.Journal.StationServices
{
    public class Repair : JournalEntryBase
    {
        #region Ship
        // TODO: check if localised?
        public string Item { get; set; } // TODO: enum: all, wear, hull, paint, or name of module
        #endregion

        #region Fleet Carrier
        public List<string> Items { get; set; }
        #endregion

        public long Cost { get; set; }
    }
}
