namespace AppData.Models
{
    public class BienThe
    {
        public Guid ID { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThai { get; set; }
        public Guid IDSanPham { get; set; }
        public Guid IDKhuyenMai { get; set; }
        public virtual IEnumerable<ChiTietBienThe> ChiTietBienThes { get; set; }
        public virtual IEnumerable<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public virtual IEnumerable<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual IEnumerable<DanhGia> DanhGias { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
    }
}
