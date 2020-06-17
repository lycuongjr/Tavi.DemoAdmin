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
using Tavi.Core.BaseController;

namespace Tavi.BoMayToChuc.Controllers
{
   public class DanhMucController: BaseController
    {
        public PartialViewResult dsCoQuan(int? id)
        {
            CoQuanService _coquan = new CoQuanService();
            IEnumerable<CoQuan> dsCoQuan = _coquan.dsCoQuan();
            return PartialView("_dmCoQuan", new SelectList(dsCoQuan, "CoQuanId", "TenCoQuan", id));
        }

        public PartialViewResult dsPhongBan(int? id, int? coquanid)
        {
            PhongBanService _phongban = new PhongBanService();
            IEnumerable<PhongBan> dsPhongBan = _phongban.DanhSach(CoQuanId: coquanid);
            return PartialView("_dmPhongBan", new SelectList(dsPhongBan, "PhongBanId", "TenPhongBan", id));
        }

        public PartialViewResult dsChucVu(int? id)
        {
            ChucVuService _chucvu = new ChucVuService();
            IEnumerable<ChucVu> dsChucVu = _chucvu.DanhSach(TrangThai: true);
            return PartialView("_dmChucVu", new SelectList(dsChucVu, "ChucVuId", "TenChucVu", id));
        }
    }
}
