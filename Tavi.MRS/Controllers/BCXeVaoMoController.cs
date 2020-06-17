using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tavi.MRS.Controllers
{
    public class BCXeVaoMoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChiTietXeVaoMo()
        {
            return View();
        }
        public ActionResult _BCXeVaoMo()
        {
            return PartialView();
        }
    }
}
