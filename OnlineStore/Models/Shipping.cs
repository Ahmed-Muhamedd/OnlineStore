namespace OnlineStore.Core.Models
{
    public class Shipping
    {
        public int ID { set; get; }
        public string CarrierName { set; get; } = null!;
        public  byte ShippingStatus { set; get; }
        public string TrackingNumber { set; get; } = null!;

        public DateTime EstimatedDeliveryDate { set; get; }
        public DateTime? ActualDeliveryDate { set; get; }

        public int OrderID { set; get; }

        public Order? Order { set; get; }
    }
}
