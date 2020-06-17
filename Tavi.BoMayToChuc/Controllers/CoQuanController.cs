using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tavi.BoMayToChuc.Models;
using Tavi.BoMayToChuc.Service;
using PagedList;
using PagedList.Mvc;
using MvcContrib.UI.Grid;
using Tavi.Core.BaseController;

namespace Tavi.BoMayToChuc.Controllers
{
    public class CoQuanController : BaseController
    {
        #region view quản lý cơ quan
        public ActionResult Index(string MaCoQuan, string TenCoQuan, int? page)
        {
            int pageCurent = page ?? 1;
            ViewBag.Page = pageCurent;
            ViewBag.hdMaCoQuan = MaCoQuan;
            ViewBag.hdTenCoQuan = TenCoQuan;
            return View();
        }

        [HttpPost]
        public PartialViewResult HienThiDanhSach(string MaCoQuan, string TenCoQuan, int PageCurent = 1)
        {
            CoQuanService _coQuan = new CoQuanService();
            IPagedList<CoQuan> dsCoQuan = _coQuan.DanhSach(MaCoQuan, TenCoQuan, PageCurent);
            ViewBag.Sort = new GridSortOptions
            {
                Column = "TenCoQuan",
                Direction = MvcContrib.Sorting.SortDirection.Ascending
            };
            return PartialView("_DanhSach", dsCoQuan);
        }

        [HttpGet]
        public ActionResult ThemMoi(int? Id)
        {
            CoQuanService _coQuan = new CoQuanService();
            CoQuan coQuan = new CoQuan();
            if (Id.HasValue)
                coQuan = _coQuan.FindByKey(Id);
            return View(coQuan);
        }
        [HttpPost]
        public ActionResult ThemMoi(int? CoQuanId
            , string MaCoQuan
            , string TenCoQuan
            , int? ThuocCoQuanId
            , string DienThoai
            , string Email
            , string DiaChi
            , string QuyetDinhThanhLap
            , string GhiChu
            , string ThuTuUuTien
            , bool ConHoatDong
            , string NgayBatDau
            , string NgayKetThuc
            , bool LaCoQuanPhatSinh)
        {
            CoQuanService _coQuan = new CoQuanService();
            var coquan = _coQuan.ThemMoi(CoQuanId
            , MaCoQuan
            , TenCoQuan
            , ThuocCoQuanId
            , DienThoai
            , Email
            , DiaChi
            , QuyetDinhThanhLap
            , GhiChu
            , ThuTuUuTien
            , ConHoatDong
            , NgayBatDau
            , NgayKetThuc
            , LaCoQuanPhatSinh
            , 0);
            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    status = coquan != null && coquan.CoQuanId > 0 ? true : false
                });
            }
            return RedirectToAction("ThemMoi", new { Id = coquan.CoQuanId});
        }
        #endregion

        #region View phòng ban trực thuộc
        public ActionResult ViewHieuChinhPhongBan(int? id)
        {
            PhongBanService _phongban = new PhongBanService();
            PhongBan phongban = new PhongBan();
            if(id.HasValue && id.Value != 0)
            {
                phongban = _phongban.FindByKey(id.Value);
            }
            return PartialView("_HieuChinhPhongBan",phongban);
        }
        #endregion

        #region View cán bộ trực thuộc
        public ActionResult ViewHieuChinhCanBo(int? id)
        {
            CanBoService _canbo = new CanBoService();
            HoSoCanBo canbo = new HoSoCanBo();
            if (id.HasValue && id.Value != 0)
            {
                canbo = _canbo.FindByKey(id.Value);
            }
            return PartialView("_HieuChinhCanBo", canbo);
        }
        #endregion

        #region View chức năng sử dụng của cơ quan
        public PartialViewResult ChucNangSuDung(int CoQuanId)
        {
            ChucNangUngDungService _chucnang = new ChucNangUngDungService();
            var danhsachChucNang = _chucnang.ChucNangSuDung_TheoCoQuan(CoQuanId);
            return PartialView("_DanhSachChucNang", danhsachChucNang);
        }

        [HttpPost]
        public JsonResult LuuChucNang(int CoQuanId, int[] dsChucNang)
        {
            ChucNangUngDungService _chucnang = new ChucNangUngDungService();
            var result = _chucnang.GhiLaiChucNangSuDung_ThuocCoQuan(CoQuanId, dsChucNang);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
