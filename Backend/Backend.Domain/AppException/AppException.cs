using Microsoft.AspNetCore.Http;

namespace Backend.Domain.AppException
{
    public class AppException: Exception
    {
        public int StatusCode { get; set; }

        public AppException(string message, int statusCode = StatusCodes.Status500InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }
      
    }
}
