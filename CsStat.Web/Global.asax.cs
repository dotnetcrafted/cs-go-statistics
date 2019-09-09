using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using CsStat.Web.Controllers;
using CSStat.WebApp.Infrastructure;

namespace CsStat.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Mapper.Initialize(cfg => cfg.AddProfiles(typeof(HomeController)));
        }
    }
}
