namespace OnlineStore.Core.Models
{
    public class Review
    {
        public int ID { set; get; }
        public string Text { set; get; } = null!;
        public byte Rating { set; get; }
        public DateTime Date { set; get; }
        public int ProductID { set; get; }
        public string CustomerID { set; get; } = null!;
        public Customer? Customer { set; get; }
        public Product? Product { set; get; }

    }

}
