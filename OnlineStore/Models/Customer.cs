namespace OnlineStore.Core.Models
{
    public class Customer
    {
        //public int ID { set; get; }
        public string UserID { set; get; } = null!;
        public ICollection<Order>? Orders { set; get; }
        public ICollection<Review>? Reviews { set; get; }
    }
}
