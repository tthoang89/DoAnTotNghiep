using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface INhanVienService
    {
        public bool Add(string ten, string email, string sdt, string diachi, Guid idVaiTro, int trangthai, string password);
        public NhanVien GetById(Guid id);
        public bool Delete(Guid id);
        public bool Update(NhanVien nv);
        public List<NhanVien> GetAll();
    }
}
