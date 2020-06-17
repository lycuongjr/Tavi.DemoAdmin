using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavi.Core.WebUtilities;

namespace Tavi.Core.BaseController
{
    public class DanhMucChungController : Controller
    {
        public ActionResult DonVihanhChinhCap1(int? id)
        {
            var dsDonVi = DanhMucApiService.DanhSach_DVHC_Cap1();
            var results = dsDonVi.Select(m => new
            {
                text = m.TenDonVi,
                id = m.DonViHCCap1Id,
                code = m.MaSoDonVi,
                selected = m.DonViHCCap1Id == id
            }).ToList();
            return Json( results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DonVihanhChinhCap2(int? id, int? parentId)
        {
            parentId = parentId.HasValue ? parentId : 0;
            var dsDonVi = DanhMucApiService.DanhSach_DVHC_Cap2(parentId);
            var results = dsDonVi.Select(m => new
            {
                text = m.TenDonVi,
                id = m.DonViHCCap2Id,
                code = m.MaSoDonVi,
                selected = m.DonViHCCap2Id == id
            }).ToList();
            return Json( results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DonVihanhChinhCap3(int? id, int? parentId)
        {
            parentId = parentId.HasValue ? parentId : 0;
            var dsDonVi = DanhMucApiService.DanhSach_DVHC_Cap3(parentId);
            var results = dsDonVi.Select(m => new
            {
                text = m.TenDonVi,
                id = m.DonViHCCap3Id,
                code = m.MaSoDonVi,
                selected = m.DonViHCCap3Id == id
            }).ToList();
            return Json( results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DonViTinh(int? id)
        {
            var dsDonvitinh = DanhMucApiService.DanhSach_DonViTinh();
            return Json(dsDonvitinh.Select(m => new
            {
                text = m.TenDonVi,
                name = m.MaDonVi,
                id = m.DonViTinhId,
                selected = m.DonViTinhId == id
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoaiVanBan()
        {
            var dsLoaiVanBan = DanhMucApiService.DanhSach_LoaiVanBan();
            return Json(dsLoaiVanBan.Select(m => new
            {
                id = m.LoaiVanBanHanhChinhId,
                text = m.TenLoaiVanBan
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LinhVuc()
        {
            var dsLinhVuc = DanhMucApiService.DanhSach_LinhVuc();
            return Json(dsLinhVuc.Select(m => new
            {
                id = m.LinhVucId,
                text = m.TenLinhVuc
            }), JsonRequestBehavior.AllowGet);
        }

    }
}
