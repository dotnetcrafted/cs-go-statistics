using CsStat.SystemFacade.DummyCache;

namespace CsStat.SystemFacade.DummyCacheFactories
{
    public class MatchDummyCacheFactory : IDummyCacheFactory
    {
        public BaseDummyCache CreateDummyCache()
        {
            return new MatchDummyCache();
        }
    }
}