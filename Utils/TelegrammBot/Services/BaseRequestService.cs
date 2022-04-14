using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public class BaseRequestService
    {
        public async Task<RequestResult<T>> GetAsync<T>(string url) where T : new() 
        {
            HttpStatusCode code;
            var content = new T();
            var error = string.Empty;

            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                var response = await client.ExecuteGetAsync<T>(request);
                code = response.StatusCode;
                content = response.Data;
            }
            catch (HttpRequestException e)
            {
                code = HttpStatusCode.InternalServerError;
                error = e.Message;
            }

            return new RequestResult<T>
            {
                Code = code,
                ErrorMessage = error,
                Content = content
            };
        }
    }
}