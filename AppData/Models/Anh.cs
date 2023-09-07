using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Anh
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public Guid IDBienThe { get; set; }
        public int TrangThai { get; set; }
        public virtual BienThe BienThe { get; set; }
    }
}
