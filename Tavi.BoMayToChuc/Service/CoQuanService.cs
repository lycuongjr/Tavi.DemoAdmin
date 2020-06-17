using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavi.Database;
using Tavi.BoMayToChuc.Models;
using Tavi.Core.UI;
using PagedList;
using Tavi.BoMayToChuc.ModelEx;

namespace Tavi.BoMayToChuc.Service
{
  public class CoQuanService : EntityHelper<CoQuan,BoMayToChucDb>
    {
        public IQueryable<CoQuan> DanhSach(string TenCoQuan)
        {
            return dbContext.CoQuans.Where(m => m.DaXoa == false && (string.IsNullOrEmpty(TenCoQuan) || m.TenCoQuan.Contains(TenCoQuan)));
        }

        /// <summary>
        /// Get danh sach co quan
        /// </summary>
        /// <param name="MaCoQuan">Ma co quan</param>
        /// <param name="TenCoQuan">Ten co quan</param>
        /// <param name="PageCurrent">Trang hien tai</param>
        /// <returns>Danh sach co quan</returns>
        public IPagedList<CoQuan> DanhSach(string MaCoQuan, string TenCoQuan, int PageCurrent)
        {
            return DanhSach(MaCoQuan,TenCoQuan, PageCurrent,10);
        }

        /// <summary>
        /// Get danh sach co quan (co phan trang)
        /// </summary>
        /// <param name="MaCoQuan">Ma co quan</param>
        /// <param name="TenCoQuan">Ten co quan</param>
        /// <param name="PageCurrent">Trang hien tai</param>
        /// <param name="PageSize">So ban ghi tren trang</param>
        /// <returns></returns>
        public IPagedList<CoQuan> DanhSach(string MaCoQuan,string TenCoQuan, int PageCurrent, int PageSize)
        {
            var danhsach = dbContext.CoQuans.Where(m => m.DaXoa == false).AsQueryable();
            if(!string.IsNullOrEmpty(MaCoQuan))
            {
                danhsach = danhsach.Where(m => m.MaCoQuan.Contains(MaCoQuan)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(TenCoQuan))
            {
                danhsach = danhsach.Where(m => m.TenCoQuan.Contains(TenCoQuan)).AsQueryable();
            }
            return danhsach.OrderByDescending(m => m.CoQuanId).ToPagedList(PageCurrent, PageSize);
        }
        public CoQuan ThemMoi(int? CoQuanId
            ,string MaCoQuan
            ,string TenCoQuan
            ,int? ThuocCoQuanId
            ,string DienThoai
            ,string Email
            ,string DiaChi
            ,string QuyetDinhThanhLap
            ,string GhiChu
            ,string ThuTuUuTien
            ,bool ConHoatDong
            ,string NgayBatDau
            ,string NgayKetThuc
            ,bool LaCoQuanPhatSinh
            ,int CanBoTaoId)
        {
            CoQuanService _coQuan = new CoQuanService();
            CoQuan coQuan = new CoQuan();
            if (CoQuanId.HasValue && CoQuanId>0)
                coQuan = _coQuan.FindByKey(CoQuanId);
            coQuan.MaCoQuan = MaCoQuan;
            coQuan.TenCoQuan = TenCoQuan;
            coQuan.ThuocCoQuanId = ThuocCoQuanId;
            coQuan.DienThoai = DienThoai;
            coQuan.Email = Email;
            coQuan.DiaChi = DiaChi;
            coQuan.QuyetDinhThanhLap = QuyetDinhThanhLap;
            coQuan.GhiChu = GhiChu;
            coQuan.ThuTuUuTien = TypeHelper.ToInt32(ThuTuUuTien);
            coQuan.ConHoatDong = ConHoatDong;
            coQuan.LaCoQuanPhatSinh = LaCoQuanPhatSinh;
            if(LaCoQuanPhatSinh == true)
            {
                if (!string.IsNullOrEmpty(NgayBatDau))
                    coQuan.NgayBatDau = TypeHelper.ToDate(NgayBatDau);
                if (!string.IsNullOrEmpty(NgayKetThuc))
                    coQuan.NgayKetThuc = TypeHelper.ToDate(NgayKetThuc);
            } else
            {
                coQuan.NgayBatDau = null;
                coQuan.NgayKetThuc = null;
            }
            
            coQuan.CanBoTaoId = CanBoTaoId;
            if (CoQuanId.HasValue && CoQuanId > 0)
                _coQuan.Update(coQuan);
            else
            {
                coQuan.DaXoa = false;
                coQuan.NgayTao = DateTime.Now;
                _coQuan.Insert(coQuan);
            }
            return coQuan;
        }
        public void Xoa(int? CoQuanId,int CanBoXoaId)
        {
            if(CoQuanId.HasValue)
            {
                CoQuanService _coQuan = new CoQuanService();
                CoQuan coQuan = _coQuan.FindByKey(CoQuanId);
                if(coQuan!=null)
                {
                    coQuan.DaXoa = true;
                    coQuan.CanBoXoaId = CanBoXoaId;
                    _coQuan.Update(coQuan);
                }
            }
        }
        public CoQuan ChiTiet(int? CoQuanId)
        {
            CoQuanService _coQuan = new CoQuanService();
            CoQuan coQuan = _coQuan.FindByKey(CoQuanId);
            return coQuan;
        }
        public CoQuan ChiTiet(int?CoQuanId,bool Proxy)
        {
            BoMayToChucDb db = new BoMayToChucDb();
            db.Configuration.ProxyCreationEnabled = Proxy;
            var coquan = db.CoQuans.Find(CoQuanId);
            return coquan;
        }
        public IEnumerable<CoQuan> dsCoQuan(int? ThuocCoQuanId)
        {
            BoMayToChucDb db = new BoMayToChucDb();
            var coquan = db.CoQuans.Where(m => (m.ThuocCoQuanId == ThuocCoQuanId || ThuocCoQuanId ==null) && m.DaXoa == false && m.ConHoatDong == true).OrderBy(m=>m.ThuTuUuTien);
            return coquan;
        }
        public IEnumerable<CoQuan> dsCoQuan()
        {
            BoMayToChucDb db = new BoMayToChucDb();
            var coquan = db.CoQuans.Where(m =>  m.DaXoa == false && m.ConHoatDong == true).OrderBy(m => m.ThuTuUuTien);
            return coquan;
        }
    }
}
