namespace CsStat.SystemFacade.DummyCache
{
    public class StatDummyCache : BaseDummyCache
    {
        public override string DependencyKey => "stat";

        public override string BuildKey(string key)
        {
            return  $"{DependencyKey}|{key}";
        }
    }

}