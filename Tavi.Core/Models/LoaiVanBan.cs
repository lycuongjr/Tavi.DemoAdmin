using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavi.Core.Models
{
    public class LoaiVanBan
    {
        public int LoaiVanBanHanhChinhId { get; set; }
        public int? SoThuTu { get; set; }
        public string MaLoaiVanBan { get; set; }
        public string TenLoaiVanBan { get; set; }
        public string ChuVietTat { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime NgayBanHanh { get; set; }
        public string CoQuanBanHanh { get; set; }
        public bool? ConSuDung { get; set; }
        public bool? DaXoa { get; set; }
    }
}
