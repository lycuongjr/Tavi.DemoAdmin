using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tavi.MRS.Controllers
{
    public class DanhSachDoanhNghiepController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }
       public ActionResult ChiTietDoanhNghiep()
        {
            return View();
        }
       public ActionResult ChiTietXe()
        {
            return View();
        }
        public ActionResult ChiTietGiayPhep()
        {
            return View();
        }
        public ActionResult ChiTietThietBi()
        {
            return View();
        }
        public ActionResult ChiTietTaiKhoan()
        {
            return View();
        }
        public ActionResult ChiTietThuTucHC()
        {
            return View();
        }
        public ActionResult _LinhVucHoatDong()
        {
            return PartialView();
        }
        public ActionResult _DanhSachMo()
        {
            return PartialView();
        }
        public ActionResult _DSXeVanChuyen()
        {
            return PartialView();
        }
        public ActionResult _GiayPhepKhaiThac()
        {
            return PartialView();
        }
        public ActionResult _ThietBiTaiMo()
        {
            return PartialView();
        }
        public ActionResult _TaiKhoanQuanTri()
        {
            return PartialView();
        }
        public ActionResult _ThuTucHanhChinh()
        {
            return PartialView();
        }
        public ActionResult _QuanLyCauHinh()
        {
            return PartialView();
        }
    }
}
