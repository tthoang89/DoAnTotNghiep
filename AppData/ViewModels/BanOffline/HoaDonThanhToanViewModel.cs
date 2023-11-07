using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class HoaDonThanhToanViewModel
    {
        public Guid Id { get; set; }
        public string KhachHang { get; set; }
        public string NhanVien { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public int TongSL { get; set; }
        public int TongTien { get; set; }
        public string GiamGia { get; set; }
        public string TrangThai { get; set; }

    }
}
