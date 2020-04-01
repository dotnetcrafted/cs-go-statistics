using System;
using System.Linq;
using System.Reflection;
using CsStat.SystemFacade.Attributes;

namespace CsStat.SystemFacade.Extensions
{
    public static class TypeExtensions
    {
        public static PropertyInfo[] GetFilteredProperties(this Type type)
        {
            return type.GetProperties()
                .Where(pi => Attribute.IsDefined(pi, typeof(IncludePropertyToJsonAttribute)))
                .ToArray();
        }
    }
}