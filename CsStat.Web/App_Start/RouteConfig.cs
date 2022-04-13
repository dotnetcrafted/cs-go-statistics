using System.Web.Mvc;
using System.Web.Routing;
using CsStat.Domain;
using CsStat.Web.Controllers;

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
                defaults: new { controller = "HangoutBot", action = "GetPlayerStat", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "playerslist",
                url: Settings.PlayersListApiPath,
                defaults: new { controller = "HangoutBot", action = "GetPlayerList", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "weaponstat",
                url: Settings.WeaponsDataApiPath,
                defaults: new { controller = "Weapon", action = "GetWeaponsStat", id = UrlParameter.Optional }
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
                name: "matches",
                url: Settings.MatchesPagePath+"/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "serverinfo",
                url: Settings.ServerInfoDataApiPath,
                defaults: new { controller = "ServerInfo", action = "ServerInfo", id = UrlParameter.Optional }
            );
                
            routes.MapRoute(
                name: "demo-reader",
                url: Settings.DemoReaderPagePath,
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "getFullMatchesData",
                url: Settings.FullMatchesDataApiPath,
                defaults: new { controller = "Matches", action = "GetFullData", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "getMatchesData",
                url: Settings.MatchesDataApiPath,
                defaults: new { controller = "Matches", action = "GetMatchesData", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "getMatchData",
                url: Settings.MatchDataApiPath,
                defaults: new { controller = "Matches", action = "GetMatch", id = UrlParameter.Optional }
            );

            routes.MapRoute
            (
                name: "clearPlayersCache",
                url: Settings.ClearPlayersCacheApi,
                defaults: new { controller = "Home", action = "ClearCache" }
            );

            routes.MapRoute
            (
                name: "bestplayerstat",
                url: Settings.BestPlayerApiPath,
                defaults: new { controller = "HangoutBot", action = "GetTodayBestPlayers", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
