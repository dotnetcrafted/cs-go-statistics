using DataService.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataService
{
    public class ConnectionStringFactory : IConnectionStringFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString() => _configuration.GetConnectionString("ConnectionString");
    }
}