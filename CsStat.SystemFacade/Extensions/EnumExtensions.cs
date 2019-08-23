using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using CsStat.SystemFacade.Attributes;

namespace CSStat.CsLogsApi.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (!(e is Enum))
                return null;

            var type = e.GetType();
            var values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val != e.ToInt32(CultureInfo.InvariantCulture))
                    continue;

                var memInfo = type.GetMember(type.GetEnumName(val));

                if (memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    return descriptionAttribute.Description;
                }
            }

            return null; 
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }

        public static List<AttributeModel> GetAttributeList<T>(this T e) where T : IConvertible
        {
            if (!(e is Enum))
                return null;

            var type = e.GetType();
            var values = Enum.GetValues(type);
            var result = new List<AttributeModel>();

            foreach (Enum value in values)
            {
                result.Add(GetAttributeWithIndex(value));
            }

            return result;
        }

        private static AttributeModel GetAttributeWithIndex( Enum e)
        {
            var index = (int)(object) e;

            var attribute = e.GetAttribute<LogValueAttribute>();

            var value = attribute == null
                ? string.Empty
                : attribute.Value;

            return new AttributeModel
            {
                Key=index,
                Value = value
            };
        }

        public class AttributeModel
        {
            public int Key { get; set; }
            public string Value { get; set; }
        }
    }
}
