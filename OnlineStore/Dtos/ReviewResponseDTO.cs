namespace OnlineStore.Dtos
{
    public class ReviewResponseDTO
    {
        public int ReviewID { set; get; }
        public string CustomerName { set; get; } = null!;
        public string Text { set; get; } = null!;
        public DateTime ReviewDate { set; get; }
        public byte Rating { set; get; }

        public string ProductName { set; get; } = null!;
    }
}
