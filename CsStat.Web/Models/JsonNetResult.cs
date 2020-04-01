using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace CsStat.Web.Models
{
    public class JsonNetResult : JsonResult
    {
        private static JsonSerializerSettings _settings;
        public JsonNetResult(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                "GET".Equals(context.HttpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }

            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            var scriptSerializer = JsonSerializer.Create(_settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, Data);
                response.Write(sw.ToString());
            }
        }
    }
}