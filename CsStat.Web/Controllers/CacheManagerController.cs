using System.Web.Mvc;
using CsStat.SystemFacade.DummyCache;
using CsStat.SystemFacade.DummyCacheFactories;

namespace CsStat.Web.Controllers
{
    public class CacheManagerController : BaseController
    {
        private readonly IDummyCacheManager _statDummyCacheManager;
        private readonly IDummyCacheManager _weaponsDummyCacheManager;
        public CacheManagerController()
        {
            _statDummyCacheManager = new DummyCacheManager(new StatDummyCacheFactory());
            _weaponsDummyCacheManager = new DummyCacheManager(new WeaponsStatDummyCacheFactory());
        }

        public ActionResult ClearCache()
        {
            _statDummyCacheManager.CacheCleanByDependency(SystemFacade.Constants.Cache.DependencyKeys.AllPlayers);
            _weaponsDummyCacheManager.CacheCleanByDependency(SystemFacade.Constants.Cache.DependencyKeys.AllWeapons);
            return Json("Ok");
        }
    }
}