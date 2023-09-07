namespace AppData.Models
{
    public class NguoiDung
    {
        public Guid IDNguoiDung { get; set; }
        public string Ten { get; set; }
        public string TenDem { get; set; }
        public string Ho { get; set; }
        public string Password { get; set; }
        public int GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public int DiemTich { get; set; }
        public int TrangThai { get; set; }
        public Guid IDVaiTro { get; set; }
        public virtual GioHang? GioHang { get; set; }
        public virtual VaiTro VaiTro { get; set; }
        public virtual IEnumerable<LichSuTichDiem> LichSuTichDiems { get; set; }
        public virtual IEnumerable<HoaDon> HoaDons { get; set; }
    }
}
