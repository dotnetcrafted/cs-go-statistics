using System;
using System.Web;
using System.Web.Caching;

namespace CsStat.SystemFacade.DummyCache
{
    public abstract class BaseDummyCache
    {
        private static Cache Cache => HttpRuntime.Cache;
        private static TimeSpan SessionTimeout => TimeSpan.FromMinutes(HttpContext.Current.Session.Timeout);

        public abstract string DependencyKey { get;}
        public abstract string BuildKey(string key);

        public virtual void AddDependency(string key)
        {
            Cache.Insert(key, string.Empty, null, Constants.Cache.NoAbsoluteExpiration, SessionTimeout);
            HttpContext.Current.Response.AddCacheItemDependencies(new[] { key });
        }

        public virtual void CacheCleanByDependency(string dependencyKey)
        {
            HttpRuntime.Cache.Remove(dependencyKey);
        }
    }
}