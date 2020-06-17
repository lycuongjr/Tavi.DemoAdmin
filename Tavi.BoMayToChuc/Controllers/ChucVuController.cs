using MvcContrib.UI.Grid;
using System;
using System.Web.Mvc;
using Tavi.BoMayToChuc.Models;
using Tavi.BoMayToChuc.Service;
using Tavi.Core.BaseController;

namespace Tavi.BoMayToChuc.Controllers
{
    public class ChucVuController : BaseController
    {
        public ActionResult Index(string MaChucVu, string TenChucVu, int? Page, GridSortOptions Sort)
        {
            ViewBag.hdMaChucVu = MaChucVu;
            ViewBag.hdTenChucVu = TenChucVu;
            ViewBag.hdPage = Page ?? 1;
            if(Sort.Column == null)
            {
                Sort.Column = "MaChucVu";
                Sort.Direction = MvcContrib.Sorting.SortDirection.Ascending;
            }
            ViewBag.Sort = Sort;
            return View();
        }

        public PartialViewResult HieuChinh(int? id)
        {
            var chucvu = new ChucVu();
            if (id.HasValue && id.Value > 0)
            {
                var _chucvu = new ChucVuService();
                chucvu = _chucvu.FindByKey(id);
            }
            return PartialView("_HieuChinh", chucvu);
        }

        public JsonResult GhiLai(ChucVu model)
        {
            try
            {
                ChucVuService _chucvu = new ChucVuService();
                var result = _chucvu.InsertOrUpdate(model.ChucVuId, model.MaChucVu, model.TenChucVu, model.ConHoatDong, model.ThuTuUuTien, null);
                return Json(new { status = result }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ThayDoiTrangThai(int? id)
        {
            if (id.HasValue)
            {
                ChucVuService _chucvu = new ChucVuService();
                var chucvu = _chucvu.FindByKey(id.Value);
                if (chucvu != null)
                {
                    chucvu.ConHoatDong = chucvu.ConHoatDong.HasValue ? !chucvu.ConHoatDong : true;
                    _chucvu.Update(chucvu);
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false, message = "Chức vụ không khả dụng" });
        }

        public JsonResult XoaChucVu(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var _chucvu = new ChucVuService();
                    var result = _chucvu.XoaChucVu(id.Value);
                    return Json(new { status = true, message = "Chức vụ đã được xóa" });
                }
                return Json(new { status = false, message = "Chức vụ không khả dụng" });
            } catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
            
        }

        #region get data
        public PartialViewResult HienThiDanhSach(string MaChucVu, string TenChucVu, int? Page)
        {
            ChucVuService _chucvu = new ChucVuService();
            int currentPage = Page.HasValue && Page > 0 ? Page.Value : 1;
            var danhsach = _chucvu.DanhSach(MaChucVu, TenChucVu, currentPage);
            if(Page.HasValue && danhsach.PageCount < Page.Value)
            {
                danhsach = _chucvu.DanhSach(MaChucVu, TenChucVu, danhsach.PageCount);
            }
            return PartialView("_DanhSach", danhsach);
        }
#endregion
    }
}
