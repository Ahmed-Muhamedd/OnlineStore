using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos
{
    public class CategoryResDto
    {
        public int ID { set; get; }
        public string CategoryName { set; get; } = null!;
    }
}
