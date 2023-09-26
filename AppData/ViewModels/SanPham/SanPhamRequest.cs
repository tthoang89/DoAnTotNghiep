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
        public string MoTa { get; set; }
        [Required]
        public int TrangThai { get; set; }
        [Required]
        public Guid IDLoaiSP { get; set; }
        public List<Guid> ListIdThuocTinh { get; set; }
    }
}
