namespace AppData.Models
{
    public class GioHang
    {
        //add-migration InitialMigration
        public Guid IDNguoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual IEnumerable<ChiTietGioHang> ChiTietGioHangs { get; set; }
    }
}
