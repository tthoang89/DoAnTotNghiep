namespace AppData.Models
{
    public class Voucher
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int HinhThucGiamGia { get; set; }
        public int SoTienCan { get; set; }
        public int GiaTri { get; set; }
        public DateTime NgayApDung { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int SoLuong { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public virtual IEnumerable<HoaDon> HoaDons { get; set; }
        //Git
    }
}
