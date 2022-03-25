namespace CsStat.SystemFacade.DummyCache
{
    public class MatchDummyCache : BaseDummyCache
    {
        public override string DependencyKey => "matchId";

        public override string BuildKey(string id)
        {
            return $"{DependencyKey}|{id}";
        }
    }
}