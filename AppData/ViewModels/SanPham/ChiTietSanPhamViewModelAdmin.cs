using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class ChiTietSanPhamViewModelAdmin
    {
        public string MaMauSac { get; set; }
        public string TenKichCo { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; }
        public DateTime NgayTao { get; set; }
        public string TenKhuyenMai { get; set; }
    }
}
