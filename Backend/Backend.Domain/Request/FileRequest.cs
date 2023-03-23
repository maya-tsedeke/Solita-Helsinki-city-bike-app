using Microsoft.AspNetCore.Http;
namespace Backend.Domain.Request
{
    public class FileRequest
    {
        public IFormFile CsvFile { get; set; }
    }
}
