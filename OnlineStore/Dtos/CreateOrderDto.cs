using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos
{
    public class CreateOrderDto
    {

        public List<OrderDetail> OrderDetials { set; get; } = null!;

        [Required]
        public string PaymentMethod { set; get; } = null!;

   
    }
    public class OrderDetail
    {
        [Required]
        public int ProductID { set; get; }

        [Required , Range(1 , int.MaxValue)]
        public int Quantity { set; get; }
    }
}
