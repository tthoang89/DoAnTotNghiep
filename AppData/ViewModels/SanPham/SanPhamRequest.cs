using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class SanPhamRequest
    {
        public Guid ID { get; set; }

        [Required]
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public int Giaban { get; set; } 
        public string DuongDanAnh { get; set; } 
        public string MoTa { get; set; }
        [Required]
        public int TrangThai { get; set; }
        public string TenChatLieu { get; set; }
        public string MaMauSac { get; set; }
        public string? TenMauSac { get; set; }
        public string TenKichCo { get; set; }
        [Required]
        public string TenLoaiSPCha { get; set; }
        public string TenLoaiSPCon { get; set; }
    }
}
