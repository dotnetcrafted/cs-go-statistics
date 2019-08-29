using System.Configuration;

namespace CsStat.SystemFacade
{
    public static class SiteConfigurations
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    }
}