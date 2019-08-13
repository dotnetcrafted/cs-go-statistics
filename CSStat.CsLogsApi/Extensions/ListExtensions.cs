using System.Collections.Generic;
using System.Linq;

namespace CSStat.CsLogsApi.Extensions
{
    public static class ListExtensions
    {
        public static List<string> Filter(this List<string> list, string condition)
        {
            var conditions = list.Where(x => x.Contains(condition));
            return list.Except(conditions).ToList();
        }
    }
}