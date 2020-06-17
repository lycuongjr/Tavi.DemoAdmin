using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tavi.MRS.Controllers
{
    public class ManHinhTheoDoiController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChiTietDoanhNghiep()
        {
            return View();
        }
        public ActionResult FullManHinhTheoDoi()
        {
            return View();
        }
        public ActionResult TheoDoiCamera()
        {
            return View();
        }
        public ActionResult ChiTietTruLuong()
        {
            return View();
        }
        public ActionResult XemBanDo()
        {
            return View();
        }
        public ActionResult GuiThongBao()
        {
            return View();
        }
        public ActionResult TaoBienBanXuPhat()
        {
            return View();
        }
        public ActionResult _DSThietBiMKN()
        {
            return PartialView();
        }
        public ActionResult _DSNoiDungCanhBao(string type)
        {
            // get data by type
            return PartialView();
        }
        public ActionResult _CameraHA()
        {
            return PartialView();
        }
        public ActionResult _BieuDoTruLuong()
        {
            return PartialView();
        }
        public ActionResult _TruLuongTheoDN()
        {
            return PartialView();
        }
    }
}
