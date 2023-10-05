using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface INhanVienService
    {
        Task<List<NhanVien>> RegisterNhanVien(NhanVienViewmodel nhanVien);
        public NhanVien GetById(Guid id);
        public bool Delete(Guid id);
        public bool Update(NhanVien nv);
        public List<NhanVien> GetAll();

    }
}
