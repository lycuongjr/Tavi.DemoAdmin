using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tavi.MRS.Controllers
{
    public class BCDanhSachDoanhNghiepController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _BCDanhSachDN()
        {
            return PartialView();
        }
    }
}
