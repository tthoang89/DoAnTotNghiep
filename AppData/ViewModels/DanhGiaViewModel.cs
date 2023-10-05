using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class DanhGiaViewModel
    {
        public Guid ID { get; set; }
        public string BinhLuan { get; set; }
        public int Sao { get; set; }
        public int TrangThai { get; set; }
        public Guid IDBienThe { get; set; }
        public string TenKH { get; set; }
    }
}
