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
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public string LoaiSP { get; set; }
        public List<string> ListImage { get; set; }
        public int GiaBan { get; set; }
        public int GiaGoc { get; set; }
        public List<ThuocTinh> ThuocTinhs { get; set; } 
        public List<BienTheViewModel> BienThes { get; set; }
    }
}
