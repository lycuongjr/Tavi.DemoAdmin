using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaviV4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Hello , it me";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page. dsdgdt";

            return View();
        }
    }
}