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
  public class NhomNguoiDungService : EntityHelper<NhomNguoiDung,BoMayToChucDb>
    {
        public IQueryable<NhomNguoiDung> DanhSach(int CoQuanId, string TenNhom)
        {
            return dbContext.NhomNguoiDungs.Where(m => m.CoQuanId == CoQuanId && (string.IsNullOrEmpty(TenNhom) || m.TenNhomNguoiDung.Contains(TenNhom)));
        }

        public NhomNguoiDung InsertOrUpdate(NhomNguoiDung nhomNguoiDung)
        {
            if (nhomNguoiDung == null) return null;
            try
            {
                if (nhomNguoiDung.NhomNguoiDungId > 0)
                {
                    var data = FindByKey(nhomNguoiDung.NhomNguoiDungId);
                    data.TenNhomNguoiDung = nhomNguoiDung.TenNhomNguoiDung;
                    data.MoTa = nhomNguoiDung.MoTa;
                    // xoa phan quyen theo nhom neu thay doi coquanId
                    if (data.CoQuanId != nhomNguoiDung.CoQuanId)
                    {
                        // lay role theo coquanId moi
                        var compareData = dbContext.UngDung_CoQuan.Where(m => m.CoQuanId == nhomNguoiDung.CoQuanId).Select(m => m.ChucNangId);
                        var oldData = dbContext.ChucNangThuocNhomNguoiDungs.Where(m => m.NhomNguoiDungId == nhomNguoiDung.NhomNguoiDungId && !compareData.Contains(m.ChucNangId));
                        dbContext.ChucNangThuocNhomNguoiDungs.RemoveRange(oldData);
                    }
                    data.CoQuanId = nhomNguoiDung.CoQuanId;
                    data.ConHoatDong = nhomNguoiDung.ConHoatDong;
                    this.Update(data);
                    return data;
                }
                else
                {
                    Insert(nhomNguoiDung);
                    return nhomNguoiDung;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
