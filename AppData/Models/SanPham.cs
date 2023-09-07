using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class SanPham
    {
        public Guid ID { get; set; }
        [StringLength(40, ErrorMessage = "Ten san pham khong duoc dai qua 40 tu.")]
        public string Ten { get; set; }
        [Required]
        public string MoTa { get; set; }

        public int TrangThai { get; set; }
        public Guid IDLoaiSP { get; set; }
        public virtual IEnumerable<BienThe> BienThes { get; set; }
        public virtual LoaiSP LoaiSP { get; set; }
    }
}
