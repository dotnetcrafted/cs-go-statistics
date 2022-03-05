using CsStat.SystemFacade.DummyCache;

namespace CsStat.SystemFacade.DummyCacheFactories
{
    public class StatDummyCacheFactory : IDummyCacheFactory
    {
        public BaseDummyCache CreateDummyCache()
        {
            return new StatDummyCache();
        }
    }
}