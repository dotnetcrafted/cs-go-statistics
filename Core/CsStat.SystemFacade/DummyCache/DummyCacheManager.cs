using CsStat.SystemFacade.DummyCacheFactories;

namespace CsStat.SystemFacade.DummyCache
{
    public interface IDummyCacheManager
    {
        void AddDependency(string key);
        void CacheCleanByDependency(string key);
    }
    public class DummyCacheManager : IDummyCacheManager
    {
        private readonly BaseDummyCache _dummyCache;

        public DummyCacheManager(IDummyCacheFactory factory)
        {
            _dummyCache = factory.CreateDummyCache();
        }

        public void AddDependency(string key)
        {
            _dummyCache.AddDependency(_dummyCache.BuildKey(key));
        }

        public void CacheCleanByDependency(string key)
        {
            _dummyCache.CacheCleanByDependency(_dummyCache.BuildKey(key));
        }
    }
}