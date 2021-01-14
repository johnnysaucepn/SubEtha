namespace Howatworks.SubEtha.Journal.FleetCarriers
{
    public class CarrierTradeOrder : JournalEntryBase
    {
        public long CarrierID { get; set; }
        public bool BlackMarket { get; set; }
        public string Commodity { get; set; }
        public string Commodity_Localised { get; set; } // NOTE: not in docs
        public int? PurchaseOrder { get; set; }
        public int? SaleOrder { get; set; }
        public bool? CancelTrade { get; set; }
        public long Price { get; set; } // TODO: check data type
    }
}
