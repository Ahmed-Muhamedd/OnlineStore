namespace OnlineStore.Core.Models
{
    public class Payment
    {
        public int ID { set; get; }
        public decimal Amount { set; get; }
        public string PaymentMethod { set; get; } = null!;

        public DateTime TransactionDate { set; get; }

        public int OrderID { set; get; }

        public Order? Order { set; get; } 
    }
}
