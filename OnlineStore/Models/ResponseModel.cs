namespace OnlineStore.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { set; get; } = null!;

        public object Data { set; get; } = null!;
    }
}
