using CsStat.SystemFacade.DummyCache;

namespace CsStat.SystemFacade.DummyCacheFactories
{
    public class WeaponsStatDummyCacheFactory : IDummyCacheFactory
    {
        public BaseDummyCache CreateDummyCache()
        {
            return new WeaponStatDummyCache();
        }
    }
}