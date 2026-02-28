namespace OnlineStore.Core.Models
{
    public class ProductImage
    {
        public int ID { set; get; }
        public string Url { set; get; } = null!;
        public int ProductID { set; get; }
        public Product? Product { set; get; }
    }

}
