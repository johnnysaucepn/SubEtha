namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    public class CarrierBankTransfer : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public long? Deposit { get; set; } // NOTE: one or other of Deposit or Withdraw will be populated
        public long? Withdraw { get; set; }
        public long PlayerBalance { get; set; }
        public long CarrierBalance { get; set; }
    }
}
