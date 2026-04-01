namespace TA_API.Models.Responses
{
    /// <summary>
    /// Standard API response wrapper.
    /// </summary>
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string? Error { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse<T> Success(T data, int statusCode = 200)
        {
            return new ApiResponse<T> { Data = data, StatusCode = statusCode };
        }

        public static ApiResponse<T> Failure(string error, int statusCode = 400)
        {
            return new ApiResponse<T> { Error = error, StatusCode = statusCode };
        }
    }
}
