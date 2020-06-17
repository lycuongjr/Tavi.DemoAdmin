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
    public class ChucNangUngDungService : EntityHelper<ChucNangUngDung, BoMayToChucDb>
    {
        public IQueryable<string> ChucNangSuDung(int NguoiDungid)
        {
            var listRoleOfUser = (from cnnd in dbContext.ChucNangNguoiDungs.Where(m => m.CanBoId == NguoiDungid)
                                    join cn in dbContext.ChucNangUngDungs.Where(m => m.ConHoatDong == true) on cnnd.ChucNangId equals cn.ChucNangId
                                    select cn.MaChucNang).Distinct();
            var listRoleOfGroup = (from cnn in dbContext.ChucNangThuocNhomNguoiDungs
                                   join gr in dbContext.CanBoThuocNhomNguoiDungs.Where(m => m.CanBoId == NguoiDungid) on cnn.NhomNguoiDungId equals gr.NhomNguoiDungId
                                   join cn in dbContext.ChucNangUngDungs.Where(m => m.ConHoatDong == true) on cnn.ChucNangId equals cn.ChucNangId
                                   select cn.MaChucNang).Distinct();
            var result = listRoleOfUser.Union(listRoleOfGroup).Distinct();
            return result;
        }

        public IEnumerable<ChucNangNguoiDungEntity> ChucNangSuDung_TheoNguoiDung(int NguoiDungid)
        {
            var result = new List<ChucNangNguoiDungEntity>();

            var coquanId = dbContext.HoSoCanBoes.Find(NguoiDungid).CoQuanHienTaiId;
            // tat ca nhom nguoi dung co nguoidungid
            var listNhomNguoiDung = dbContext.CanBoThuocNhomNguoiDungs.Where(m => m.CanBoId == NguoiDungid).Select(m=>m.NhomNguoiDungId);

            var listData = (from cn in dbContext.ChucNangUngDungs
                                 join cq in dbContext.UngDung_CoQuan.Where(m => m.CoQuanId == coquanId) on cn.ChucNangId equals cq.ChucNangId
                                 join cnnd in dbContext.ChucNangNguoiDungs.Where(m => m.CanBoId == NguoiDungid) on cn.ChucNangId equals cnnd.ChucNangId into temp_cnnd
                                 from tb_cnnd in temp_cnnd.DefaultIfEmpty()
                                 join cnn in dbContext.ChucNangThuocNhomNguoiDungs.Where(m => listNhomNguoiDung.Contains(m.NhomNguoiDungId)) on cn.ChucNangId equals cnn.ChucNangId into temp_cnn
                                 from tb_cnn in temp_cnn.DefaultIfEmpty()
                                 join nnd in dbContext.NhomNguoiDungs.Where(m => m.CoQuanId == coquanId) on tb_cnn.NhomNguoiDungId equals nnd.NhomNguoiDungId into temp_nnd
                                 from tb_nnd in temp_nnd.DefaultIfEmpty()
                                 group new { cn, tb_cnnd, tb_cnn, tb_nnd } by new { cn.ChucNangId, cn.TenChucNang, cn.MaChucNang, cn.ThuocChucNangId, tb_cnnd.CanBoId } into tb
                                 select new ChucNangNguoiDungEntity
                                 {
                                     ChucNangId = tb.Key.ChucNangId,
                                     TenChucNang = tb.Key.TenChucNang,
                                     MaChucNang = tb.Key.MaChucNang,
                                     ThuocChucNangId = tb.Key.ThuocChucNangId,
                                     TenNhomNguoiDung = tb.Select(m => m.tb_nnd.TenNhomNguoiDung),
                                     IsActive = tb.Key.CanBoId != null || tb.Select(m => m.tb_nnd.TenNhomNguoiDung).Where(m => m != null).Count() > 0,
                                     HasGroupRole = tb.Select(m => m.tb_nnd.TenNhomNguoiDung).Where(m => m != null).Count() > 0
                                 }).ToList();
            
            if (listData == null || listData.Count <= 0)
            {
                return result;
            }

            return AddChildItems(listData, null, -1);
        }

        public IEnumerable<ChucNangEntity> ChucNangSuDung_TheoNhomNguoiDung(int NhomNguoiDungId, int CoQuanId)
        {
            var result = new List<ChucNangEntity>();
            var listData = (from cn in dbContext.ChucNangUngDungs.Where(m => m.ConHoatDong == true)
                            join cncq in dbContext.UngDung_CoQuan.Where(m => m.CoQuanId == CoQuanId) on cn.ChucNangId equals cncq.ChucNangId
                            join cq in dbContext.ChucNangThuocNhomNguoiDungs.Where(m => m.NhomNguoiDungId == NhomNguoiDungId) on cn.ChucNangId equals cq.ChucNangId into ud_cq
                            from ud in ud_cq.DefaultIfEmpty()
                            select new ChucNangEntity
                            {
                                ChucNangId = cn.ChucNangId,
                                MaChucNang = cn.MaChucNang,
                                TenChucNang = cn.TenChucNang,
                                ThuocChucNangId = cn.ThuocChucNangId,
                                IsActive = ud.NhomNguoiDungId.HasValue
                            }).ToList();
            if (listData == null || listData.Count <= 0)
            {
                return result;
            }

            return AddChildItems(listData, null, -1);
        }

        public IEnumerable<ChucNangEntity> ChucNangSuDung_TheoCoQuan(int coquanID)
        {
            var result = new List<ChucNangEntity>();
            var listData = (from cn in dbContext.ChucNangUngDungs.Where(m => m.ConHoatDong == true)
                            join cq in dbContext.UngDung_CoQuan.Where(m => m.CoQuanId == coquanID) on cn.ChucNangId equals cq.ChucNangId into ud_cq
                            from ud in ud_cq.DefaultIfEmpty()
                            select new ChucNangEntity
                            {
                                ChucNangId = cn.ChucNangId,
                                MaChucNang = cn.MaChucNang,
                                TenChucNang = cn.TenChucNang,
                                ThuocChucNangId = cn.ThuocChucNangId,
                                IsActive = ud.CoQuanId.HasValue
                            }).ToList();
            if (listData == null || listData.Count <= 0)
            {
                return result;
            }

            return AddChildItems(listData, null, -1);
        }

        public List<ChucNangEntity> AddChildItems(List<ChucNangEntity> listData, int? parentID, int currentLevel)
        {
            var result = new List<ChucNangEntity>();
            var listParent = listData.Where(m => m.ThuocChucNangId == parentID);
            foreach (var item
                in listParent)
            {
                item.Level = currentLevel + 1;
                result.Add(item);
                result.AddRange(AddChildItems(listData, item.ChucNangId, item.Level));
            }
            return result;
        }

        public List<ChucNangNguoiDungEntity> AddChildItems(List<ChucNangNguoiDungEntity> listData, int? parentID, int currentLevel)
        {
            var result = new List<ChucNangNguoiDungEntity>();
            var listParent = listData.Where(m => m.ThuocChucNangId == parentID);
            foreach (var item
                in listParent)
            {
                item.Level = currentLevel + 1;
                result.Add(item);
                result.AddRange(AddChildItems(listData, item.ChucNangId, item.Level));
            }
            return result;
        }

        public bool GhiLaiChucNangSuDung_ThuocCoQuan(int coquanID, int[] dsChucNang)
        {
            // delete old data
            var listData = dbContext.UngDung_CoQuan.Where(m => m.CoQuanId == coquanID);
            dbContext.UngDung_CoQuan.RemoveRange(listData);

            // insert new data
            if (dsChucNang != null && dsChucNang.Length > 0)
            {
                var listNewData = new List<UngDung_CoQuan>();
                foreach (var item in dsChucNang)
                {
                    listNewData.Add(new UngDung_CoQuan { ChucNangId = item, CoQuanId = coquanID });
                }
                dbContext.UngDung_CoQuan.AddRange(listNewData);
            }
            return dbContext.SaveChanges() > 0;
        }

        public bool GhiLaiChucNangSuDung_ThuocNhomNguoiDung(int nhomnguoidungId, int[] dsChucNang)
        {
            // delete old data
            var listData = dbContext.ChucNangThuocNhomNguoiDungs.Where(m => m.NhomNguoiDungId == nhomnguoidungId);
            dbContext.ChucNangThuocNhomNguoiDungs.RemoveRange(listData);

            // insert new data
            if (dsChucNang != null && dsChucNang.Length > 0)
            {
                var listNewData = new List<ChucNangThuocNhomNguoiDung>();
                foreach (var item in dsChucNang)
                {
                    listNewData.Add(new ChucNangThuocNhomNguoiDung { ChucNangId = item, NhomNguoiDungId = nhomnguoidungId });
                }
                dbContext.ChucNangThuocNhomNguoiDungs.AddRange(listNewData);
            }
            return dbContext.SaveChanges() > 0;
        }

        public bool GhiLaiChucNangSuDung_TheoNguoiDung(int canboId, int[] dsChucNang)
        {
            // delete old data
            var listData = dbContext.ChucNangNguoiDungs.Where(m => m.CanBoId == canboId);
            dbContext.ChucNangNguoiDungs.RemoveRange(listData);

            // insert new data
            if (dsChucNang != null && dsChucNang.Length > 0)
            {
                var listNewData = new List<ChucNangNguoiDung>();
                foreach (var item in dsChucNang)
                {
                    listNewData.Add(new ChucNangNguoiDung { ChucNangId = item, CanBoId = canboId });
                }
                dbContext.ChucNangNguoiDungs.AddRange(listNewData);
            }
            return dbContext.SaveChanges() > 0;
        }

    }
}
