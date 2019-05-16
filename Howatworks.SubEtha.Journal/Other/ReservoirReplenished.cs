namespace Howatworks.SubEtha.Journal.Other
{
    public class ReservoirReplenished : JournalEntryBase
    {
        public decimal FuelMain { get; set; }
        public decimal FuelReservoir { get; set; }
    }
}
