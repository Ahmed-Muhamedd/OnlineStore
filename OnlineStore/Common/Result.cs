namespace OnlineStore.Common
{
    public class Result<T>
    {
        public T? Data { get; }
        public string? ErrorMessage { get; }

        public ResultStatus Status { get; }

        private Result(T? data , string? errorMessage, ResultStatus status)
        {
            Data = data;
            ErrorMessage = errorMessage;
            Status = status;
        }

        public static Result<T> Success(T? data) 
            => new Result<T>(data, null , ResultStatus.Success);
        public static Result<T> NotFound(string? errorMessage) 
            => new Result<T>(default, errorMessage , ResultStatus.NotFound);

        public static Result<T> BadRequest(string? errorMessage)
        => new Result<T>(default, errorMessage, ResultStatus.BadRequest);
        public static Result<T> ServerError(string? errorMessage)
        => new Result<T>(default, errorMessage, ResultStatus.ServerError);
        //public static Result<T> NotFound(string? errorMessage)
        //=> new Result<T>(default, errorMessage, ResultStatus.NotFound);


        public bool IsSuccess => Status == ResultStatus.Success;
    }

    public enum ResultStatus
    {
        Success = 200,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        ServerError = 500
    }
}
