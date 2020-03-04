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

            routes.LowercaseUrls = true;

            routes.MapRoute
            (
                name: "getrepository",
                url: Settings.PlayersDataApiPath,
                defaults: new { controller = "Home", action = "GetRepository", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "playerstat",
                url: Settings.PlayerStatApiPath,
                defaults: new { controller = "Home", action = "GetPlayerStat", id = UrlParameter.Optional }
            );

            //don't remove: custom admin
            //routes.MapRoute
            //(
            //    name: "Admin",
            //    url: "Admin",
            //    defaults: new { controller = "SignIn", action = "SignIn", id = UrlParameter.Optional }
            //);

            routes.MapRoute
            (
                name: "Admin",
                url: "Admin",
                defaults: new { controller = "SignIn", action = "Admin", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "getinfo",
                url: Settings.WikiDataApiPath,
                defaults: new { controller = "Wiki", action = "GetAllArticlesFromCms", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "wiki",
                url: Settings.WikiPagePath,
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "serverinfo",
                url: Settings.ServerInfoDataApiPath,
                defaults: new { controller = "ServerInfo", action = "ServerInfo", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "serverinfomock",
                url: Settings.ServerInfoDataMockApiPath,
                defaults: new { controller = "ServerInfo", action = "ServerInfoMock", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "serverinfowidget",
                url: Settings.ServerInfoDataWidgetApiPath,
                defaults: new { controller = "ServerInfo", action = "ServerInfoWidget", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
