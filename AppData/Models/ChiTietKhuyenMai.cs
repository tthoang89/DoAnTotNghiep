namespace AppData.Models
{
    public class ChiTietKhuyenMai
    {
        public Guid ID { get; set; }
        public int TrangThai { get; set; }
        public Guid IDBienThe { get; set; }
        public Guid IDKhuyenMai { get; set; }
        public virtual BienThe BienThe { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
    }
}
