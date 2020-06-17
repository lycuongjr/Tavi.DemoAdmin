using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavi.BoMayToChuc.Models;
using Tavi.BoMayToChuc.Service;
using Tavi.Core.BaseController;

namespace Tavi.BoMayToChuc.Controllers
{
    public class NhomNguoiDungController : BaseController
    {
        public ActionResult Index(int? id)
        {
            ViewBag.SelectCoQuanID = id;
            return View();
        }

        #region Get data
        public PartialViewResult HienThiDanhSach(int CoQuanId, string TenNhom)
        {
            NhomNguoiDungService _nhomnguoidung = new NhomNguoiDungService();
            var dsNhom = _nhomnguoidung.DanhSach(CoQuanId,TenNhom).ToList();
            return PartialView("_DanhSachNhomNguoiDung", dsNhom);
        }

        public PartialViewResult DanhSachCoQuan(string TenCoQuan, int? CoQuanID)
        {
            CoQuanService _coquan = new CoQuanService();
            var dsCoQuan = _coquan.DanhSach(TenCoQuan).ToList();
            return PartialView("_DanhSachCoQuan", dsCoQuan);
        }
        #endregion

        #region update thông tin nhóm
        public ActionResult HieuChinh(int? id)
        {
            NhomNguoiDung nhom = new NhomNguoiDung();
            if(id.HasValue && id.Value > 0)
            {
                NhomNguoiDungService _nhom = new NhomNguoiDungService();
                nhom = _nhom.FindByKey(id.Value);
            }
            return PartialView("_HieuChinhNhom", nhom);
        }
        public ActionResult LuuHieuChinh(NhomNguoiDung nhomnguoidung)
        {
            NhomNguoiDungService _nhom = new NhomNguoiDungService();
            var nhom = _nhom.InsertOrUpdate(nhomnguoidung);
            return Json(new
            {
                success = nhom != null,
                data = nhom
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region phân quyền theo nhóm
        public PartialViewResult DanhSachChucNang(int CoQuanId, int NhomNguoiDungId)
        {
            ChucNangUngDungService _chucnang = new ChucNangUngDungService();
            var data = _chucnang.ChucNangSuDung_TheoNhomNguoiDung(NhomNguoiDungId, CoQuanId);
            return PartialView("_DanhSachChucNang", data);
        }

        [HttpPost]
        public JsonResult LuuChucNang(int NhomNguoiDungId, int[] dsChucNang)
        {
            ChucNangUngDungService _chucnang = new ChucNangUngDungService();
            var result = _chucnang.GhiLaiChucNangSuDung_ThuocNhomNguoiDung(NhomNguoiDungId, dsChucNang);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sach nguoi dung thuoc nhom nguoi dung
        public PartialViewResult DanhSachNguoiDung(int NhomNguoiDungId)
        {
            CanBoService _canbo = new CanBoService();
            var data = _canbo.DanhSach_TheoNhomNguoiDung(NhomNguoiDungId).OrderBy(m => m.TenPhongBan).ThenBy(m => m.TenChucVu).ToList();
            return PartialView("_DanhSachNguoiDung", data);
        }

        public PartialViewResult DanhSachNguoiDungThemMoi(int NhomNguoiDungId, int CoQuanId)
        {
            CanBoService _canbo = new CanBoService();
            var data = _canbo.DanhSach_NhapMoiNhomNguoiDung(CoQuanId, NhomNguoiDungId).OrderBy(m=>m.TenPhongBan).ThenBy(m => m.TenChucVu).ToList();
            return PartialView("_DanhSachNguoiDungNhapMoi", data);
        }

        public JsonResult ThemNguoiDung(int NhomNguoiDungId, int[] dsNguoiDung)
        {
            CanBoThuocNhomNguoiDungService _service = new CanBoThuocNhomNguoiDungService();
            var data = _service.ThemNguoiDungVaoNhom(NhomNguoiDungId, dsNguoiDung);
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult XoaNguoiDung(int NhomNguoiDungId, int NguoiDungId, bool? XoaTatCa)
        {
            CanBoThuocNhomNguoiDungService _service = new CanBoThuocNhomNguoiDungService();
            var result = false;
            if(XoaTatCa == true)
            {
                result = _service.XoaNguoiDung(NhomNguoiDungId);
            }
            else
            {
                result = _service.XoaNguoiDung(NhomNguoiDungId, NguoiDungId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
