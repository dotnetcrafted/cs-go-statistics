using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsStat.Web.Models;

namespace CsStat.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserModel user)
        {
            var userName = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];

            if (string.Equals(userName, user.UserName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(password, user.Password))
            {
                Session["UserName"] = userName;
                return RedirectToAction("Index","Admin");
            }

            ViewBag.NotValidUser = "Incorrect username or password";
            return View();
        }
    }
}