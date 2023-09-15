namespace AppData.Models
{
    public class ThuocTinhSanPham
    {
        public Guid ID { get; set; }
        public Guid IDThuocTinh { get; set; }
        public Guid IDSanPham { get; set; }
        public virtual ThuocTinh ThuocTinh { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}
