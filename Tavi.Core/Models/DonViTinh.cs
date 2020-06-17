using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavi.Core.Models
{
    public class DonViTinh
    {
        public int DonViTinhId { get; set; }
        public int SoThuTu { get; set; }
        public string MaDonVi { get; set; }
        public string TenDonVi { get; set; }
        public bool? ConSuDung { get; set; }
        public bool? DaXoa { get; set; }
    }
}
