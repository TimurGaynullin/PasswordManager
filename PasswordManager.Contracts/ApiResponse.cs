namespace PasswordManager.Contracts
{
    public class ApiResponse<T> : ApiResponse where T : class
    {
        public ApiResponse()
        {
        }

        public ApiResponse(T result) => this.Result = result;

        public T Result { get; set; }
    }
    
    public class ApiResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public int ErrorCode { get; set; }

        public static ApiResponse CreateSuccess() => new ApiResponse()
        {
            Success = true
        };

        public static ApiResponse<TResult> CreateSuccess<TResult>(TResult result) where TResult : class
        {
            var apiResponse = new ApiResponse<TResult>
            {
                Result = result,
                Success = true
            };
            return apiResponse;
        }

        public static ApiResponse CreateFailure(string error = null, int? errorCode = null) => new ApiResponse()
        {
            Success = false,
            Error = error,
            ErrorCode = errorCode ?? 0
        };

        public static ApiResponse<TResult> CreateFailure<TResult>(
            string error = null,
            int? errorCode = null)
            where TResult : class
        {
            ApiResponse<TResult> apiResponse = new ApiResponse<TResult>();
            apiResponse.Success = false;
            apiResponse.Error = error;
            apiResponse.ErrorCode = errorCode ?? 0;
            return apiResponse;
        }
    }
}