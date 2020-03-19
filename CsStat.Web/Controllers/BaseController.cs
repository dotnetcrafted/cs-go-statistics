using System.Web.Mvc;
using CsStat.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CsStat.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override JsonResult Json(
            object data,
            string contentType,
            System.Text.Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            var options = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };

            return new JsonNetResult(options)
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}