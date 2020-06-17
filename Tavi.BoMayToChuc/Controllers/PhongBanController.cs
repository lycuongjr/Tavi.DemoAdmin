using System;
using System.Web.Mvc;
using PagedList;
using Tavi.BoMayToChuc.Models;
using Tavi.BoMayToChuc.Service;
using Tavi.Core.BaseController;
using Tavi.Core.Constants;

namespace Tavi.BoMayToChuc.Controllers
{
    public class PhongBanController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GhiLai(PhongBan phongban)
        {
            PhongBanService _phongban = new PhongBanService();
            var result = _phongban.InsertOrUpdate(phongban);
            return Json(new { status = result != null });
        }

        public JsonResult ThayDoiTrangThai(int? id)
        {
            if (id.HasValue)
            {
                PhongBanService _phongban = new PhongBanService();
                var phongban = _phongban.FindByKey(id.Value);
                if (phongban != null)
                {
                    phongban.ConHoatDong = phongban.ConHoatDong.HasValue ? !phongban.ConHoatDong : true;
                    _phongban.Update(phongban);
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false, message = "Phòng ban không khả dụng" });
        }

        public JsonResult XoaPhongBan(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var _phongban = new PhongBanService();
                    var result = _phongban.XoaPhongBan(id.Value);
                    return Json(new { status = true, message = "Phòng ban đã được xóa" });
                }
                return Json(new { status = false, message = "Phòng ban không khả dụng" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }

        public ActionResult DanhSachTrucThuocCoQuan(int? CoQuanId, int Page = 1)
        {
            PhongBanService _phongban = new PhongBanService();
            var dsPhanTrang = _phongban.DanhSachTrucThuocCoQuan(CoQuanId, Page);
            return Json(new { count = dsPhanTrang.TotalItemCount, content = RenderRazorViewToString("_DanhSachTrucThuocCoQuan", dsPhanTrang) }, JsonRequestBehavior.AllowGet);
        }
    }
}
