using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class KhachHang
    {
        public Guid IDKhachHang { get; set; }
        public string Ten { get; set; }
        public string Password { get; set; }
        public int GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public int DiemTich { get; set; }
        public int TrangThai { get; set; }
        public Guid IDVaiTro { get; set; }
        public virtual GioHang? GioHang { get; set; }
        public virtual IEnumerable<LichSuTichDiem> LichSuTichDiems { get; set; }
        public virtual IEnumerable<DanhGia> DanhGias { get; set; }
    }
}
