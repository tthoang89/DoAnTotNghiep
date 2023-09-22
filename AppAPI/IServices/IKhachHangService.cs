using AppData.Models;

namespace AppAPI.IServices
{
    public interface IKhachHangService
    {
        public bool Add(KhachHang nv);
        public KhachHang GetById(Guid id);
        public bool Delete(Guid id);
        public bool Update(KhachHang nv);
        public List<KhachHang> GetAll();
    }
}
