namespace CsStat.SystemFacade.DummyCache
{
    public class StatDummyCache : BaseDummyCache
    {
        public override string DependencyKey => "stat";

        public override string BuildKey(string key)
        {
            return  $"{DependencyKey}|{key}";
        }

        public override void CacheCleanByDependency(string dependencyKey)
        {
            var enumerator = System.Web.HttpRuntime.Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var key = (string)enumerator.Key;

                if (key != null && key.Contains(dependencyKey))
                {
                    System.Web.HttpRuntime.Cache.Remove(key);
                }
            }

        }
    }

}