namespace AppData.Models
{
    public class ChiTietBienThe
    {
        public Guid ID { get; set; }
        public int TrangThai { get; set; }
        public Guid IDBienThe { get; set; }
        public Guid IDGiaTri { get; set; }
        public virtual BienThe BienThe { get; set; }
        public virtual GiaTri GiaTri { get; set; }
    }
}
