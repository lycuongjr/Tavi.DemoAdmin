using System;
using System.Collections.Generic;
using System.Linq;
using Tavi.BoMayToChuc.Models;
using Tavi.Database;

namespace Tavi.BoMayToChuc.Service
{
    public class CanBoThuocNhomNguoiDungService : EntityHelper<CanBoThuocNhomNguoiDung,BoMayToChucDb>
    {
        public bool ThemNguoiDungVaoNhom(int NhomNguoiDungId, int[] dsNguoiDung)
        {
            if (dsNguoiDung == null || dsNguoiDung.Length <= 0) 
            return false;
            var listData = new List<CanBoThuocNhomNguoiDung>();
            foreach (var user in dsNguoiDung)
            {
                listData.Add(new CanBoThuocNhomNguoiDung { CanBoId = user, NhomNguoiDungId = NhomNguoiDungId });
            }
            Insert(listData);
            return true;
        }

        public bool XoaNguoiDung(int NhomNguoiDungId, int NguoiDungId)
        {
            var listData = dbContext.CanBoThuocNhomNguoiDungs.Where(m => m.NhomNguoiDungId == NhomNguoiDungId && m.CanBoId == NguoiDungId);
            if (listData.Count() > 0)
            {
                dbContext.CanBoThuocNhomNguoiDungs.RemoveRange(listData);
                SaveChanges();
            }
            return true;
        }

        public bool XoaNguoiDung(int NhomNguoiDungId)
        {
            var listData = dbContext.CanBoThuocNhomNguoiDungs.Where(m => m.NhomNguoiDungId == NhomNguoiDungId);
            if (listData.Count() > 0)
            {
                dbContext.CanBoThuocNhomNguoiDungs.RemoveRange(listData);
                SaveChanges();
            }
            return true;
        }
    }
}
