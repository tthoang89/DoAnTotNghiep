using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface IKhachHangService
    {
        public Task<KhachHang> Add(KhachHangViewModel nv);
        public KhachHang GetById(Guid id);
        public bool Delete(Guid id);
        public bool Update(Guid id, string email , string password);
        public List<KhachHang> GetAll();
    }
}
