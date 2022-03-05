using CsStat.SystemFacade.DummyCache;

namespace CsStat.SystemFacade.DummyCacheFactories
{
    public interface IDummyCacheFactory
    {
        BaseDummyCache CreateDummyCache();
    }
}