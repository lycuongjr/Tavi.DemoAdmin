using PagedList;
using System;
using System.Linq;
using Tavi.BoMayToChuc.Models;
using Tavi.Core.Constants;
using Tavi.Database;

namespace Tavi.BoMayToChuc.Service
{
    public class ChucVuService : EntityHelper<ChucVu, BoMayToChucDb>
    {
        /// <summary>
        /// Get tat ca chuc vu
        /// </summary>
        /// <param name="MaChucVu">Ma chuc vu</param>
        /// <param name="TenChucVu">Ten chuc vu</param>
        /// <returns>Danh sach chuc vu</returns>
        public IQueryable<ChucVu> DanhSach(string MaChucVu = null, string TenChucVu = null, bool? TrangThai = null)
        {
            var danhsach = dbContext.ChucVus.Where(m => m.DaXoa != true
                                                    && (!TrangThai.HasValue || m.ConHoatDong == TrangThai)
                                                    && (string.IsNullOrEmpty(MaChucVu) || m.MaChucVu.Contains(MaChucVu))
                                                    && (string.IsNullOrEmpty(TenChucVu) || m.TenChucVu.Contains(TenChucVu))
                                                ).OrderBy(m => m.ThuTuUuTien);
            return danhsach;
        }

        public IPagedList<ChucVu> DanhSach(string MaChucVu, string TenChucVu, int pageNumber, int pageSize = Constants.Default_PageSize)
        {
            var danhsach = DanhSach(MaChucVu, TenChucVu);
            return danhsach.ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật chức vụ
        /// </summary>
        /// <param name="chucvu">Thông tin chức vụ</param>
        /// <returns></returns>
        public bool InsertOrUpdate(ChucVu chucvu)
        {
            if (chucvu == null) return false;
            try
            {
                if(chucvu.ChucVuId > 0)
                {
                    Update(chucvu);
                }
                else
                {
                    Insert(chucvu);
                }
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật chức vụ
        /// </summary>
        /// <param name="ChucVuId">Chức vụ ID</param>
        /// <param name="MaChucVu">Mã chức vụ</param>
        /// <param name="TenChucVu">Tên chức vụ</param>
        /// <param name="ConHoatDong">Còn hoạt động</param>
        /// <param name="ThuTuUuTien">Thứ tự ưu tiên</param>
        /// <param name="UserId">Người đăng nhập hiện tại</param>
        /// <returns></returns>
        public bool InsertOrUpdate(int ChucVuId, string MaChucVu, string TenChucVu, bool? ConHoatDong, int? ThuTuUuTien, int? UserId)
        {
            var chucvu = new ChucVu();
            if (ChucVuId > 0)
            {
                chucvu = FindByKey(ChucVuId);
            } else
            {
                chucvu.NgayTao = DateTime.Now;
                chucvu.CanBoTaoId = UserId;
            }
            chucvu.MaChucVu = MaChucVu;
            chucvu.TenChucVu = TenChucVu;
            chucvu.ConHoatDong = ConHoatDong;
            chucvu.ThuTuUuTien = ThuTuUuTien;
            // call funtion update
            return InsertOrUpdate(chucvu);
        }

        public bool XoaChucVu(int chucvuid, int? UserId = null)
        {
            try
            {
                var chucvu = FindByKey(chucvuid);
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
