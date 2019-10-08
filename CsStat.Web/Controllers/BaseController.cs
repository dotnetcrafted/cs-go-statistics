using System.Web.Mvc;
using CsStat.Web.Models;

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
            return new JsonNetResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
            };
        }
    }
}