namespace OnlineStore.Core.Models
{
    public class ProductCategory
    {
        public int ID { set; get; }
        public string Name { set; get; } = null!;

        public ICollection<Product>? Products { set; get; }

    }

}
