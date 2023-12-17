using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class MauSac
    {
        [Required]
        public Guid? ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên màu sắc")]
        [StringLength(20, ErrorMessage = "Tên màu sắc không được vượt quá 10 kí tự ")]
        public string? Ten { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn mã màu")]
        public string? Ma { get; set; }
        public int? TrangThai { get; set; }
        public virtual IEnumerable<ChiTietSanPham>? ChiTietSanPhams { get; set; }
        public virtual IEnumerable<Anh>? Anhs { get; set; }
    }
}
