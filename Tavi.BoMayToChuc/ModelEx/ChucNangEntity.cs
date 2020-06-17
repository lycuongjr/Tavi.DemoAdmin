using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavi.BoMayToChuc.Models;

namespace Tavi.BoMayToChuc.ModelEx
{
    public class ChucNangEntity : ChucNangUngDung
    {
        public bool IsActive { get; set; }
        public int Level { get; set; }
    }

    public class ChucNangNguoiDungEntity : ChucNangEntity
    {
        public IEnumerable<string> TenNhomNguoiDung { get; set; }
        public int ChucNangNguoiDungId { get; set; }
        public bool HasGroupRole { get; set; }
    }
}
