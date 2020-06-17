using System;
using System.Web.Mvc;
using Tavi.BoMayToChuc.Membership;
using Tavi.BoMayToChuc.ModelEx;
using Tavi.BoMayToChuc.Models;
using Tavi.BoMayToChuc.Service;
using Tavi.Core.BaseController;
using Tavi.Core.UI;

namespace Tavi.BoMayToChuc.Controllers
{
    public class CanBoController : BaseController
    {
        #region View quản lý cán bộ
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult HienThiDanhSach(string HoVaTen, string TenDangNhap, int? Page)
        {
            var currentPage = Page ?? 1;
            CanBoService _canbo = new CanBoService();
            var danhsach = _canbo.DanhSachPhanTrang(HoVaTen, TenDangNhap, currentPage);
            return PartialView("_DanhSach", danhsach);
        }

        public ActionResult ThemMoi(int? id)
        {
            CanBoService _canbo = new CanBoService();
            var canbo = new HoSoCanBo();
            if(id.HasValue && id.Value > 0)
            {
                canbo = _canbo.FindByKey(id.Value);
            }
            return View(canbo);
        }

        public ActionResult GhiLai(HoSoCanBo canbo)
        {
            CanBoService _canbo = new CanBoService();
            var result = _canbo.InsertOrUpdate(canbo);
            if (Request.IsAjaxRequest())
            {
                return Json(new { status = result != null });
            }
            return RedirectToAction("Index");
        }

        public JsonResult ThayDoiTrangThai(int? id)
        {
            if (id.HasValue)
            {
                CanBoService _canbo = new CanBoService();
                var canbo = _canbo.FindByKey(id.Value);
                if (canbo != null)
                {
                    canbo.ConHoatDong = canbo.ConHoatDong.HasValue ? !canbo.ConHoatDong : true;
                    _canbo.Update(canbo);
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false, message = "Cán bộ không khả dụng" });
        }

        public JsonResult XoaCanBo(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var _canbo = new CanBoService();
                    var result = _canbo.XoaCanBo(id.Value);
                    return Json(new { status = true, message = "Cán bộ đã được xóa" });
                }
                return Json(new { status = false, message = "Cán bộ không khả dụng" });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }

        public ActionResult DanhSachTrucThuoc(int? CoQuanId, string TimKiem, int Page = 1)
        {
            CanBoService _canbo = new CanBoService();
            var dsCanBo = _canbo.DanhSachTrucThuocCoQuan(CoQuanId, TimKiem, Page);
            //return PartialView("_DanhSachTrucThuoc", dsCanBo);
            return Json(new { count = dsCanBo.TotalItemCount, content = RenderRazorViewToString("_DanhSachTrucThuoc", dsCanBo) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region phân quyền cá nhân
        public PartialViewResult DsPhanQuyenCaNhan(int CanBoId)
        {
            ChucNangUngDungService _chucnang = new ChucNangUngDungService();
            var listChucNang = _chucnang.ChucNangSuDung_TheoNguoiDung(CanBoId);
            return PartialView("DsPhanQuyenCaNhan", listChucNang);
        }

        public JsonResult GhiLaiPhanQuyen(int CanBoId, int[] dsChucNang)
        {
            ChucNangUngDungService _chucnang = new ChucNangUngDungService();
            var result = _chucnang.GhiLaiChucNangSuDung_TheoNguoiDung(CanBoId, dsChucNang);
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thông tin cá nhân
        public ActionResult ThongTinCaNhan()
        {
            var currentUser = HttpContext.User.Identity.GetUserLogin();
            CanBoService _canbo = new CanBoService();
            var canbo = _canbo.ThongTinCanBo(currentUser.CanBoId);
            return View(canbo);
        }

        public ActionResult LuuAnhDaiDien()
        {
            var currentUser = HttpContext.User.Identity.GetUserLogin();
            var files = Request.Files;
            if (files.Count > 0)
            {
                // upload new file
                var fileName = FileHelper.UploadFile("~/Uploads/UserImages/", files);
                if (!string.IsNullOrEmpty(fileName))
                {
                    // save new filename to db
                    CanBoService _canbo = new CanBoService();
                    var canbo = _canbo.FindByKey(currentUser.CanBoId);
                    var oldImage = canbo.AnhDaiDien;
                    canbo.AnhDaiDien = fileName;
                    _canbo.Update(canbo);
                    Session.Remove("_Avatar");
                    Session["_Avatar"] = canbo.AnhDaiDien;

                    // delete old image
                    FileHelper.RemoveFile(Server.MapPath(oldImage));

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CapNhatThongTin(CapNhatThongTinCanBo canbo)
        {
            CanBoService _canbo = new CanBoService();
            var cb = _canbo.FindByKey(canbo.CanBoId);
            cb.HoVaTen = canbo.HoVaTen;
            cb.SoCMND = canbo.SoCMND;
            cb.SoTheCanCuoc = canbo.SoTheCanCuoc;
            cb.Email = canbo.Email;
            cb.DienThoai = canbo.DienThoai;
            _canbo.Update(cb);

            // update session
            Session.Remove("_FULLNAME");
            Session["_FULLNAME"] = string.IsNullOrEmpty(cb.HoVaTen) ? "" : cb.HoVaTen;
            Session.Remove("_EMAIL");
            Session["_EMAIL"] = string.IsNullOrEmpty(cb.Email) ? "" : cb.Email;
            Session.Remove("_TEL");
            Session["_TEL"] = string.IsNullOrEmpty(cb.DienThoai) ? "" : cb.DienThoai;

            return Json(new { Success = true, Message = "Đã thay đổi thông tin" }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult CapNhatMatKhau(CapNhatTaiKhoanNguoiDung taikhoan)
        {
            CanBoService _canbo = new CanBoService();
            var cb = _canbo.FindByKey(taikhoan.CanBoId);
            // check old password
            if (cb.MatKhau != EncryptHelper.EncryptMD5(taikhoan.MatKhau))
            {
                return Json(new { Success = false, Message = "Mật khẩu cũ không đúng" }, JsonRequestBehavior.DenyGet);
            }
            // update new password
            cb.MatKhau = EncryptHelper.EncryptMD5(taikhoan.MatKhauMoi);
            _canbo.Update(cb);
            return Json(new { Success = true, Message = "Đã thay đổi mật khẩu" }, JsonRequestBehavior.DenyGet);
        }
        #endregion
    }
}
