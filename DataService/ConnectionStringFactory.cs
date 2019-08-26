using CsStat.SystemFacade;
using DataService.Interfaces;

namespace DataService
{
    public class ConnectionStringFactory : IConnectionStringFactory
    {
        public string GetConnectionString() => SiteConfigurations.ConnectionString;
    }
}