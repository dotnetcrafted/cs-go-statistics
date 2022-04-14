namespace CsStat.SystemFacade.DummyCache
{
    public class WeaponStatDummyCache : BaseDummyCache
    {
        public override string DependencyKey => "weaponsStat";
        public override string BuildKey(string key)
        {
            return $"{DependencyKey}|{key}";
        }
    }
}