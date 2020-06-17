using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tavi.MRS.Controllers
{
    public class NhanThongBaoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChiTietBaoCao()
        {
            return View();
        }
        public ActionResult _BaoCaoDoanhNghiep()
        {
            return PartialView();
        }
        public ActionResult _DSThongBao()
        {
            return PartialView();
        }
        public ActionResult _NoiDungBaoCao()
        {
            return PartialView();
        }
    }
}
