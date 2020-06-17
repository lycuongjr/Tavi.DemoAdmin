using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavi.Core.Models
{
    public class LinhVuc
    {
        public int LinhVucId { get; set; }
        public int? SoThuTu { get; set; }
        public string MaLinhVuc { get; set; }
        public string TenLinhVuc { get; set; }
        public bool? ConSuDung { get; set; }
        public bool? DaXoa { get; set; }
    }
}
