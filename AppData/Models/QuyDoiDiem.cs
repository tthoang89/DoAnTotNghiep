namespace AppData.Models
{
    public class QuyDoiDiem
    {
        public Guid ID { get; set; }
        public int TiLeTichDiem { get; set; }
        public int TiLeTieuDiem { get; set; }
        public int TrangThai { get; set; }//0 là ko sử dụng,1 là chỉ tích hoặc tiêu, 2 là vừa tích vừa tiêu.
        public virtual IEnumerable<LichSuTichDiem> LichSuTichDiems { get; set; }
    }
}
