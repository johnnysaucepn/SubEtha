namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    public class CarrierShipPack : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public string Operation { get; set; } // TODO: enum (buypack/sellpack/restockpack)
        public string PackTheme { get; set; }
        public int PackTier { get; set; }
        public long? Cost { get; set; }
        public long? Refund { get; set; }
    }
}
