using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class HoaDonChiTietViewModel
    {
        public Guid Id { get; set; }
        public Guid IdHoaDon { get; set; }
        public Guid IDChiTietSanPham { get; set; }
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }

    }
}
