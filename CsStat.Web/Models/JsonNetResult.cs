using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CsStat.Web.Models
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult(object value, string contentType) 
            : base(value)
        {
            ContentType = contentType;
        }

        private static JsonSerializerSettings Settings => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Error
        };

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if ("GET".Equals(context.HttpContext.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }                   

            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : ContentType;

            if (Value == null)
                return;

            var scriptSerializer = JsonSerializer.Create(Settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, Value);
                response.Body.Write(sw.Encoding.GetBytes(sw.ToString()));
            }
        }
    }
}