namespace OnlineStore.Dtos
{
    public class OrderResponseDto
    {
        public int OrderID { set; get; }
        public DateTime EstimatedDeliveryDate { set; get; }
        public decimal TotalAmount { set; get; }

    }
}
