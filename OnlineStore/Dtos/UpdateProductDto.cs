using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos
{
    public class UpdateProductDto
    {
        [Required, MinLength(3), MaxLength(100)]
        public string Name { set; get; } = null!;

        [Required, MinLength(3), MaxLength(100)]
        public string Description { set; get; } = null!;

        [Required(ErrorMessage = "Price is required"), Range(0, (double)decimal.MaxValue, ErrorMessage = "Price must be positive or more than 0")]
        public decimal Price { set; get; }

        [Required(ErrorMessage = "Quantity In Stock is required"),
            Range(0, int.MaxValue, ErrorMessage = "Quantity In Stock must be positive or more than 0")]
        public int QuantityInStock { set; get; }

        [Required(ErrorMessage = "Category Id is required"),
            Range(1, int.MaxValue, ErrorMessage = "Category ID must be more than 0")]
        public int CategoryID { set; get; }
        public IEnumerable<ImagesDTO>? Images { set; get; }



    }
}
