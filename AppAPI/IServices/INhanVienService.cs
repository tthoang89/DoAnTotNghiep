using AppData.Models;

namespace AppAPI.IServices
{
    public interface INhanVienService
    {
        public string Add(NhanVien nv);
        public NhanVien GetById(Guid id);
        public bool Delete(Guid id);
        public bool Update(NhanVien nv);
        public List<NhanVien> GetAll();

    }
}
