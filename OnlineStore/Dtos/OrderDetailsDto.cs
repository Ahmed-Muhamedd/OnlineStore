using Azure.Core.Pipeline;

namespace OnlineStore.Dtos
{
    public class OrderDetailsDto
    {
        public int OrderID { set; get; }
        public DateTime OrderDate { set; get; }
        public decimal TotalAmount { set; get; }

        public List<ItemsDto> Items { set; get; } = null!;
    }

    public class ItemsDto
    {
        public string ProductName { set; get; } = null!;
        public int Quantity { set; get; }
        public decimal Price { set; get; }
    }
}
