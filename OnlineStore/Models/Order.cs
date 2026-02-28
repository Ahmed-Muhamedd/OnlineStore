namespace OnlineStore.Core.Models
{
    public class Order
    {
        public int ID { set; get; }
        public DateTime CreatedAt { set; get; }
        public decimal TotalAmount { set; get; }
        public byte Status { set; get; }
        public Payment? Payment { set; get; }
        public string CustomerID { set; get; } = null!;
        public Customer? Customer { set; get; } 
        public ICollection<OrderItem>? OrderItems { set; get; }

        public Shipping? Shipping { set; get; }
    }
}
