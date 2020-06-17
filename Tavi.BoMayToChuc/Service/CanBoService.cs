using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using Tavi.BoMayToChuc.ModelEx;
using Tavi.BoMayToChuc.Models;
using Tavi.Core.Constants;
using Tavi.Core.UI;
using Tavi.Database;

namespace Tavi.BoMayToChuc.Service
{
    public class CanBoService : EntityHelper<HoSoCanBo, BoMayToChucDb>
    {
        public ThongTinCanBo GetUserLogin(string username, string password)
        {
            var result = (from cb in dbContext.HoSoCanBoes.Where(m => m.ConHoatDong == true && m.DaXoa != true && m.TenDangNhap == username && m.MatKhau == password)
                         join cq in dbContext.CoQuans.Where(m => m.ConHoatDong == true && m.DaXoa != true) on cb.CoQuanHienTaiId equals cq.CoQuanId
                         join pb in dbContext.PhongBans.Where(m => m.ConHoatDong == true && m.DaXoa != true) on cb.PhongBanHienTaiId equals pb.PhongBanId
                         join cv in dbContext.ChucVus.Where(m => m.ConHoatDong == true && m.DaXoa != true) on cb.ChucVuHienTaiId equals cv.ChucVuId
                         select new ThongTinCanBo
                         {
                             CanBoId = cb.CanBoId,
                             MaCanBo = cb.MaCanBo,
                             HoVaTen = cb.HoVaTen,
                             TenDangNhap = cb.TenDangNhap,
                             MatKhau = cb.MatKhau,
                             Email = cb.Email,
                             DienThoai = cb.DienThoai,
                             PhongBanHienTaiId = cb.PhongBanHienTaiId,
                             TenPhongBan = pb.TenPhongBan,
                             ChucVuHienTaiId = cb.ChucVuHienTaiId,
                             TenChucVu = cv.TenChucVu,
                             CoQuanHienTaiId = cb.CoQuanHienTaiId,
                             TenCoQuan = cq.TenCoQuan,
                             AnhDaiDien = cb.AnhDaiDien
                         }).FirstOrDefault();
            return result;
        }

        public ThongTinCanBo ThongTinCanBo(int userId)
        {
            var result = (from cb in dbContext.HoSoCanBoes.Where(m => m.ConHoatDong == true && m.DaXoa != true && m.CanBoId == userId)
                          join cq in dbContext.CoQuans.Where(m => m.ConHoatDong == true && m.DaXoa != true) on cb.CoQuanHienTaiId equals cq.CoQuanId
                          join pb in dbContext.PhongBans.Where(m => m.ConHoatDong == true && m.DaXoa != true) on cb.PhongBanHienTaiId equals pb.PhongBanId
                          join cv in dbContext.ChucVus.Where(m => m.ConHoatDong == true && m.DaXoa != true) on cb.ChucVuHienTaiId equals cv.ChucVuId
                          select new ThongTinCanBo
                          {
                              CanBoId = cb.CanBoId,
                              MaCanBo = cb.MaCanBo,
                              HoVaTen = cb.HoVaTen,
                              TenDangNhap = cb.TenDangNhap,
                              Email = cb.Email,
                              DienThoai = cb.DienThoai,
                              PhongBanHienTaiId = cb.PhongBanHienTaiId,
                              TenPhongBan = pb.TenPhongBan,
                              ChucVuHienTaiId = cb.ChucVuHienTaiId,
                              TenChucVu = cv.TenChucVu,
                              CoQuanHienTaiId = cb.CoQuanHienTaiId,
                              TenCoQuan = cq.TenCoQuan,
                              AnhDaiDien = cb.AnhDaiDien,
                              SoCMND = cb.SoCMND,
                              SoTheCanCuoc = cb.SoTheCanCuoc
                          }).FirstOrDefault();
            return result;
        }

        public IQueryable<HoSoCanBo> DanhSach(bool? ConHoatDong = null)
        {
            var danhsach = dbContext.HoSoCanBoes.Where(m => m.DaXoa != true && (!ConHoatDong.HasValue || m.ConHoatDong == ConHoatDong));
            return danhsach;
        }

        public IPagedList<HoSoCanBo> DanhSachPhanTrang(int pageNumber = 1, int pageSize = Constants.Default_PageSize)
        {
            var danhsach = DanhSach();
            return danhsach.ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<HoSoCanBo> DanhSachPhanTrang(string HoVaTen, string TenDangNhap, int pageNumber = 1, int pageSize = Constants.Default_PageSize)
        {
            var danhsach = DanhSach();
            danhsach = danhsach.Where(m => (string.IsNullOrEmpty(HoVaTen) || m.HoVaTen.Contains(HoVaTen))
                                            && string.IsNullOrEmpty(TenDangNhap) || m.TenDangNhap.Contains(TenDangNhap))
                               .OrderBy(m => m.ThuTuUuTien);
            return danhsach.ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<HoSoCanBo> DanhSachTrucThuoc(int? CoQuanId, int? PhongBanId,int pageNumber = 1, int pageSize = Constants.Default_PageSize)
        {
            var danhsach = DanhSach().Where(m => m.DaXoa != true && (!CoQuanId.HasValue || m.CoQuanHienTaiId == CoQuanId)
                                                                     && (!PhongBanId.HasValue || m.PhongBanHienTaiId == PhongBanId))
                                         .OrderBy(m => m.CanBoId);
            return danhsach.ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<ThongTinCanBo> DanhSachTrucThuocCoQuan(int? CoQuanId, string TextSeach, int pageNumber = 1, int pageSize = Constants.Default_PageSize)
        {
            var danhsach = DanhSach().Where(m => m.DaXoa != true && (!CoQuanId.HasValue || m.CoQuanHienTaiId == CoQuanId));
            var result = (from cb in danhsach
                          join pb in dbContext.PhongBans on cb.PhongBanHienTaiId equals pb.PhongBanId
                          join cv in dbContext.ChucVus on cb.ChucVuHienTaiId equals cv.ChucVuId
                          where string.IsNullOrEmpty(TextSeach)
                                  || cb.HoVaTen.Contains(TextSeach)
                                  || cb.TenDangNhap.Contains(TextSeach)
                                  || cb.Email.Contains(TextSeach)
                                  || cb.DienThoai.Contains(TextSeach)
                                  || pb.TenPhongBan.Contains(TextSeach)
                                  || cv.TenChucVu.Contains(TextSeach)
                          orderby cb.PhongBanHienTaiId
                          select new ThongTinCanBo
                          {
                              CanBoId = cb.CanBoId,
                              MaCanBo = cb.MaCanBo,
                              HoVaTen = cb.HoVaTen,
                              TenDangNhap = cb.TenDangNhap,
                              Email = cb.Email,
                              DienThoai = cb.DienThoai,
                              SoCMND = cb.SoCMND,
                              SoTheCanCuoc = cb.SoTheCanCuoc,
                              PhongBanHienTaiId = cb.PhongBanHienTaiId,
                              TenPhongBan = pb.TenPhongBan,
                              TenChucVu = cv.TenChucVu,
                              ConHoatDong = cb.ConHoatDong
                          });

            return result.ToPagedList(pageNumber, pageSize);
        }

        public IEnumerable<ThongTinCanBo> DanhSach_TheoNhomNguoiDung(int NhomNguoiDungId)
        {
            var dsUser = DanhSach(true);
            var result = from nhom in dbContext.CanBoThuocNhomNguoiDungs.Where(m => m.NhomNguoiDungId == NhomNguoiDungId)
                         join cb in dsUser on nhom.CanBoId equals cb.CanBoId
                         join pb in dbContext.PhongBans on cb.PhongBanHienTaiId equals pb.PhongBanId
                         join cv in dbContext.ChucVus on cb.ChucVuHienTaiId equals cv.ChucVuId
                         select new ThongTinCanBo
                         {
                             CanBoId = cb.CanBoId,
                             MaCanBo = cb.MaCanBo,
                             HoVaTen = cb.HoVaTen,
                             TenDangNhap = cb.TenDangNhap,
                             PhongBanHienTaiId = cb.PhongBanHienTaiId,
                             TenPhongBan = pb.TenPhongBan,
                             TenChucVu = cv.TenChucVu
                         };
            return result;
        }

        public IEnumerable<ThongTinCanBo> DanhSach_NhapMoiNhomNguoiDung(int CoQuanId, int NhomNguoiDungId)
        {
            var dsUser = DanhSach(true);
            var useInGroup = dbContext.CanBoThuocNhomNguoiDungs.Where(m => m.NhomNguoiDungId == NhomNguoiDungId).Select(m => m.CanBoId);

            var result = from cb in dsUser.Where(m => m.CoQuanHienTaiId == CoQuanId && !useInGroup.Contains(m.CanBoId))
                         join pb in dbContext.PhongBans on cb.PhongBanHienTaiId equals pb.PhongBanId
                         join cv in dbContext.ChucVus on cb.ChucVuHienTaiId equals cv.ChucVuId
                         select new ThongTinCanBo
                         {
                             CanBoId = cb.CanBoId,
                             MaCanBo = cb.MaCanBo,
                             HoVaTen = cb.HoVaTen,
                             TenDangNhap = cb.TenDangNhap,
                             PhongBanHienTaiId = cb.PhongBanHienTaiId,
                             TenPhongBan = pb.TenPhongBan,
                             TenChucVu = cv.TenChucVu
                         };
            return result;
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật thông tin cán bộ
        /// </summary>
        /// <param name="canbo">Thông tin cán bộ</param>
        /// <returns>Kết quá lưu data</returns>
        //public bool InsertOrUpdate(HoSoCanBo canbo)
        //{
        //    if (canbo == null) return false;
        //    try
        //    {
        //        if(canbo.CanBoId > 0)
        //        {
        //            Update(canbo);
        //        }
        //        else
        //        {
        //            Insert(canbo);
        //        }
        //        return true;
        //    } catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public HoSoCanBo InsertOrUpdate(HoSoCanBo canbo)
        {
            if (canbo == null) return null;
            try
            {
                if(canbo.CanBoId > 0)
                {
                    var data = FindByKey(canbo.CanBoId);
                    data.MaCanBo = canbo.MaCanBo;
                    data.HoVaTen = canbo.HoVaTen;
                    data.TenDangNhap = canbo.TenDangNhap;
                    data.MatKhau = data.MatKhau == canbo.MatKhau ? data.MatKhau : EncryptHelper.EncryptMD5(canbo.MatKhau);
                    data.DienThoai = canbo.DienThoai;
                    data.Email = canbo.Email;
                    data.SoCMND = canbo.SoCMND;
                    data.SoTheCanCuoc = canbo.SoTheCanCuoc;
                    data.TrangThaiHoatDong = canbo.TrangThaiHoatDong;
                    data.CoQuanHienTaiId = canbo.CoQuanHienTaiId;
                    data.PhongBanHienTaiId = canbo.PhongBanHienTaiId;
                    data.ChucVuHienTaiId = canbo.ChucVuHienTaiId;
                    data.ConHoatDong = canbo.ConHoatDong;
                    data.ThuTuUuTien = canbo.ThuTuUuTien;
                    data.LoaiTaiKhoan = canbo.LoaiTaiKhoan;
                    this.Update(data);
                    return data;
                }
                else
                {
                    canbo.MatKhau = EncryptHelper.EncryptMD5(canbo.MatKhau);
                    Insert(canbo);
                    return canbo;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool XoaCanBo(int canboId, int? UserId = null)
        {
            try
            {
                var canbo = FindByKey(canboId);
                canbo.DaXoa = true;
                canbo.CanBoXoaId = UserId;
                Update(canbo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
