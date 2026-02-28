namespace OnlineStore.Dtos
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string Name { set; get; } = null!;

        public string Description { set; get; } = null!;

        public decimal Price { set; get; }
  
        public int QuantityInStock { set; get; }

        public int CategoryID { set; get; }

        public IEnumerable<ImagesDTO> Images { set; get; } = null!;

    }
}
