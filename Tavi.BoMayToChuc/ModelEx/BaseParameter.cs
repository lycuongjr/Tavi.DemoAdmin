using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavi.BoMayToChuc.ModelEx
{
    public class BaseParameter
    {
        public int? pageIndex { get; set; }
        public int? pageSize { get; set; }
        public string sortField { get; set; }
        public string sortOrder { get; set; }
    }

    public class UserSearchParameter
    {
        public string HoVaTen { get; set; }
        public string TenDangNhap { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string TenPhongBan { get; set; }
        public string TenCoQuan { get; set; }
    }

}
