using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class SanPhamViewModel
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public string LoaiSP { get; set; }
        public Guid? IdBT { get; set; } // Id của biến thể mặc định
        public List<string>? ListImage { get; set; } // Của biến thể mặc định
        public int? GiaBan { get; set; }//Của biến thể mặc định sau khi nhân vs khuyến mãi
        public int? GiaGoc { get; set; }//Của biến thể mặc định
    }
}
