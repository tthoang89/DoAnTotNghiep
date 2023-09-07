using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class ThuocTinh
    {
        public Guid ID { get; set; }
        [StringLength(50, ErrorMessage = "Ten thuoc tinh khong duoc dai qua 50 ki tu.")]
        public string Ten { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThai { get; set; }
        public virtual IEnumerable<GiaTri> GiaTris { get; set; }
        public virtual IEnumerable<ThuocTinhLoaiSP> ThuocTinhLoaiSPs { get; set; }
    }
}
