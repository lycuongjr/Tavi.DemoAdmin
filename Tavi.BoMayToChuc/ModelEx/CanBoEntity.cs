using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavi.BoMayToChuc.Models;

namespace Tavi.BoMayToChuc.ModelEx
{
    public class ThongTinCanBo : HoSoCanBo
    {
        public string TenPhongBan { get; set; }
        public string TenCoQuan { get; set; }
        public string TenChucVu { get; set; }
        public List<string> DsChucNang { get; set; }
    }

    public class CapNhatThongTinCanBo
    {
        public int CanBoId { get; set; }
        public string HoVaTen { get; set; }
        public string SoCMND { get; set; }
        public string SoTheCanCuoc { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
    }

    public class CapNhatTaiKhoanNguoiDung
    {
        public int CanBoId { get; set; }
        public string MatKhau { get; set; }
        public string MatKhauMoi { get; set; }
    }
}
