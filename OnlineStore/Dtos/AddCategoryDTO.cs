using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos
{
    public class AddCategoryDTO
    {
        [Required, MaxLength(100)]
        public string CategoryName { set; get; } = null!;

    }
}
