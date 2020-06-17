using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using Tavi.BoMayToChuc.Models;
using Tavi.Core.Constants;
using Tavi.Database;

namespace Tavi.BoMayToChuc.Service
{
    public class PhongBanService : EntityHelper<PhongBan, BoMayToChucDb>
    {
        public IQueryable<PhongBan> DanhSach(string MaPhongBan = null, string TenPhongBan = null, bool? TrangThai = null, int? CoQuanId = null)
        {
            var danhsach = dbContext.PhongBans.Where(m => m.DaXoa != true
                                                    && (!TrangThai.HasValue || m.ConHoatDong == TrangThai)
                                                    && (string.IsNullOrEmpty(MaPhongBan) || m.MaPhongBan.Contains(MaPhongBan))
                                                    && (string.IsNullOrEmpty(TenPhongBan) || m.TenPhongBan.Contains(TenPhongBan))
                                                    && (!CoQuanId.HasValue || m.ThuocCoQuanId == CoQuanId)
                                                ).OrderBy(m => m.ThuTuUuTien);
            return danhsach;
        }

        public IPagedList<PhongBan> DanhSach(string MaPhongBan, string TenPhongBan, int pageNumber, int pageSize = Constants.Default_PageSize)
        {
            var danhsach = DanhSach(MaPhongBan, TenPhongBan);
            return danhsach.ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<PhongBan> DanhSachTrucThuocCoQuan(int? CoQuanId, int Page = 1)
        {
            var dsPhongBan = DanhSach(CoQuanId: CoQuanId);
            var dsPhanTrang = dsPhongBan.ToPagedList(Page, Constants.Default_PageSize);
            return dsPhanTrang;
        }

        public PhongBan InsertOrUpdate(PhongBan phongban)
        {
            if (phongban == null) return null;
            try
            {
                if(phongban.PhongBanId > 0)
                {
                    Update(phongban);
                }
                else
                {
                    Insert(phongban);
                }
                return phongban;
            } catch (Exception)
            {
                return null;
            }
        }

        public bool XoaPhongBan(int phongbanid, int? UserId = null)
        {
            try
            {
                var chucvu = FindByKey(phongbanid);
                chucvu.DaXoa = true;
                chucvu.CanBoXoaId = UserId;
                Update(chucvu);
                return true;
            } catch (Exception)
            {
                return false;
            }
            
        }
    }
}
