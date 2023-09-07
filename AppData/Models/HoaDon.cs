namespace AppData.Models
{
    public class HoaDon
    {
        public Guid ID { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string DiaChi { get; set; }
        public int TienShip { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public int TrangThaiGiaoHang { get; set; }
        public Guid? IDNguoiDung { get; set; }
        public Guid? IDVoucher { get; set; }
        public virtual IEnumerable<LichSuTichDiem> LichSuTichDiems { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual IEnumerable<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
