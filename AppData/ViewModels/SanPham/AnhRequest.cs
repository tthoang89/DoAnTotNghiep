using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class AnhRequest
    {
        public string? DuongDan { get; set; }
        public Guid IDMauSac { get; set; }
        public string? TenMauSac { get; set; }
        public string? MaMauSac { get; set; }
        public Guid IDSanPham { get; set; }
    }
}
