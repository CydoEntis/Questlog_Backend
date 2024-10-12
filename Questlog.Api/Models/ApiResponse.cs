using System.Net;

namespace Questlog.Api.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
        public object Result { get; set; }
    }
}
