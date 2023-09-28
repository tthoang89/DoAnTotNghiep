using AppData.Models;

namespace AppAPI.IServices
{
    public interface ILishSuTichDiemServices
    {
        public bool Add(int diem,int trangthai,Guid IdKhachHang,Guid IdQuyDoiDiem, Guid IdHoaDon);
        public bool Update(Guid Id, int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon);
        public bool Delete(Guid Id);
        public LichSuTichDiem GetById(Guid Id);
        public List<LichSuTichDiem> GetAll();
    }
}
