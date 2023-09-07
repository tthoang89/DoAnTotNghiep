namespace AppData.Models
{
    public class ChiTietGioHang
    {
        public Guid ID { get; set; }
        public int SoLuong { get; set; }
        public Guid IDBienThe { get; set; }
        public Guid IDNguoiDung { get; set; }
        public virtual BienThe BienThe { get; set; }
        public virtual GioHang GioHang { get; set; }
    }
}
