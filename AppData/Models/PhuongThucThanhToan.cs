using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class PhuongThucThanhToan
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public virtual IEnumerable<ChiTietPTTT>? ChiTietPTTTs { get; set; }
    }
}
