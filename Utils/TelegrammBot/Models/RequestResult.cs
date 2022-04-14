using System.Net;

namespace TelegramBot.Models
{
    public class RequestResult<T>
    {
        public HttpStatusCode Code { get; set; }
        public string ErrorMessage { get; set; }
        public T Content { get; set; }
    }
}