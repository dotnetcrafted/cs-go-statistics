using System.Web.Mvc;
using System.Web.Routing;
using CsStat.Domain;

namespace CsStat.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute
            (
                name: "getrepository",
                url: Settings.PlayersDataApiPath,
                defaults: new { controller = "Home", action = "GetRepository", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "getMatchesData",
                url: Settings.MatchesDataApiPath,
                defaults: new { controller = "Home", action = "GetMatchesData", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Matches",
                url: "Matches",
                defaults: new { controller = "Home", action = "Matches", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
