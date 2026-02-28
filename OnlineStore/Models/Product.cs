namespace OnlineStore.Core.Models
{
    public class Product
    {
        public int ID { set; get; }
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public decimal Price { set; get; }
        public int QuantityInStock { set; get; }
        public int CategoryID { set; get; }
        public ProductCategory? ProductCategory { set; get; }
        public ICollection<ProductImage>? Images { set; get; }
        public ICollection<Review>? Reviews { set; get; }
        public ICollection<OrderItem>? OrderItems { set; get; }
    }

}
