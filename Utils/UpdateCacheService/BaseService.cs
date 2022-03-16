using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataService;
using ErrorLogger;
using RestSharp;

namespace UpdateCacheService
{
    public class BaseService
    {
        public ILogger Logger;

        public BaseService()
        {
            var connectionString = new ConnectionStringFactory();
            var mongoRepository = new MongoRepositoryFactory(connectionString);
            Logger = new Logger(mongoRepository);
        }

        public async Task<RequestResult> GetAsync(string url)
        {
            var client = new RestClient(url);
            HttpStatusCode code;
            string content;
            var request = new RestRequest(Method.GET);

            try
            {
                var response = await client.ExecuteGetAsync(request);
                code = response.StatusCode;
                content = response.Content;
            }
            catch (HttpRequestException e)
            {
                code = HttpStatusCode.InternalServerError;
                content = e.Message;
            }

            return new RequestResult
            {
                Code = code,
                Content = content
            };
        }
    }
}