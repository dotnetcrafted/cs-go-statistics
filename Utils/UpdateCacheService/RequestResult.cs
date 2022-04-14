using System.Net;

namespace UpdateCacheService
{
    public class RequestResult
    {
        public HttpStatusCode Code { get; set; }
        public string Content { get; set; }
    }
}