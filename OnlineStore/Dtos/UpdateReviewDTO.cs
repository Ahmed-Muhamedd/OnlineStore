using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos
{
    public class UpdateReviewDTO
    {

        [Required, StringLength(500, ErrorMessage = "Review text cannot exceed 500 characters")]
        public string Text { set; get; } = null!;

        [Required, Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public byte Rating { set; get; }

    }
}
