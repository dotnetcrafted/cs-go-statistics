using CsStat.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CsStat.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(IRouteBuilder routes)
        {
            routes.MapRoute
            (
                name: "getrepository",
                template: Settings.PlayersDataApiPath,
                defaults: new { controller = "Home", action = "GetRepository"}
            );

            routes.MapRoute
            (
                name: "playerstat",
                template: Settings.PlayerStatApiPath,
                defaults: new { controller = "HangoutBot", action = "GetPlayerStat" }
            );

            routes.MapRoute
            (
                name: "playerslist",
                template: Settings.PlayersListApiPath,
                defaults: new { controller = "HangoutBot", action = "GetPlayerList" }
            );

            //don't remove: custom admin
            //routes.MapRoute
            //(
            //    name: "Admin",
            //    template: "Admin",
            //    defaults: new { controller = "SignIn", action = "SignIn" }
            //);

            routes.MapRoute
            (
                name: "Admin",
                template: "Admin",
                defaults: new { controller = "SignIn", action = "Admin" }
            );

            routes.MapRoute
            (
                name: "getinfo",
                template: Settings.WikiDataApiPath,
                defaults: new { controller = "Wiki", action = "GetAllArticlesFromCms" }
            );

            routes.MapRoute(
                name: "wiki",
                template: Settings.WikiPagePath,
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "matches",
                template: Settings.MatchesPagePath+"/{id}",
                defaults: new { controller = "Home", action = "Index" }
            );


            routes.MapRoute(
                name: "serverinfo",
                template: Settings.ServerInfoDataApiPath,
                defaults: new { controller = "ServerInfo", action = "ServerInfo" }
            );
                
            routes.MapRoute(
                name: "demo-reader",
                template: Settings.DemoReaderPagePath,
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute
            (
                name: "getFullMatchesData",
                template: Settings.FullMatchesDataApiPath,
                defaults: new { controller = "Matches", action = "GetFullData" }
            );

            routes.MapRoute
            (
                name: "getMatchesData",
                template: Settings.MatchesDataApiPath,
                defaults: new { controller = "Matches", action = "GetMatchesData" }
            );

            routes.MapRoute
            (
                name: "getMatchData",
                template: Settings.MatchDataApiPath,
                defaults: new { controller = "Matches", action = "GetMatch" }
            );

            routes.MapRoute(
                name: "Default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
