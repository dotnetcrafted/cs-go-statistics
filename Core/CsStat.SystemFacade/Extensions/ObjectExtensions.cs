using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CsStat.SystemFacade.Extensions
{
    public static class ObjectExtensions
    {
        private const string DateFormatString = "yyyy-MM-ddTHH:mm:ss";

        public static string ToJson(this object anObject)
        {
            return anObject.ToJson(false, false);
        }

        public static string ToJson(this object anObject, string dateFormatString)
        {
            return anObject.ToJson(true, true, dateFormatString);
        }

        public static string ToJson(this object anObject, bool ignoreDefaultValuesHandling,
            bool ignoreNullValueHandling, string dateFormatString = DateFormatString)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = ignoreDefaultValuesHandling
                    ? DefaultValueHandling.Ignore
                    : DefaultValueHandling.Include,
                NullValueHandling = ignoreNullValueHandling ? NullValueHandling.Ignore : NullValueHandling.Include,
                DateFormatString = dateFormatString,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return anObject.ToJson(jsonSerializerSettings);
        }

        public static string ToJson(this object anObject, JsonSerializerSettings jsonSerializerSettings)
        {
            if (jsonSerializerSettings == null)
            {
                throw new ArgumentNullException(nameof(jsonSerializerSettings));
            }

            var jsonString = JsonConvert.SerializeObject(anObject, jsonSerializerSettings);

            return jsonString;
        }

        public static string ToStringSafely(this object anObject)
        {
            if (anObject == null)
            {
                return string.Empty;
            }

            return anObject.ToString();
        }
    }
}