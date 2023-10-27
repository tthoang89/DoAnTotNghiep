using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class HoaDonChiTietRequest
    {
        public Guid Id { get; set; }    
        public Guid IdHoaDon { get; set; }
        public Guid IdBienThe { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int TrangThai { get; set; }
    }
}
