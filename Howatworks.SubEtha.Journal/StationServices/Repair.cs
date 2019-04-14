namespace Howatworks.SubEtha.Journal.StationServices
{
    public class Repair : JournalEntryBase
    {
        public string Item { get; set; } // TODO: enum: all, wear, hull, paint, or name of module
        // TODO: optionally localised?
        //public string Item_Localised { get; set; }
        public long Cost { get; set; }
    }
}
