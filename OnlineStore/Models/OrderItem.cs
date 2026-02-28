namespace OnlineStore.Core.Models
{
    public class OrderItem
    {
        public int ID { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public decimal TotalItemsPrice { set; get; }
        public int OrderID { set; get; }
        public int ProductID { set; get; }

        public Order? Order { set; get; }
        public Product? Product { set; get; }

    }

}
