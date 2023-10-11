using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class AnhBienThe
    {
        public Guid ID { get; set; }
        public Guid IdAnh { get; set; }
        public Guid IdBienThe { get; set; }
        public DateTime NgayTao { get; set; }
        public virtual Anh? Anh { get; set; }
        public virtual BienThe? BienThe { get; set; }

    }
}
