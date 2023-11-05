using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface INhanVienService
    {
        public bool Add(string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro);
        public NhanVien? GetById(Guid id);
        public IEnumerable<NhanVien> GetByName(string name);
        public bool Delete(Guid id);
        public bool Update(Guid id, string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro);
        public List<NhanVien> GetAll();
    }
}
