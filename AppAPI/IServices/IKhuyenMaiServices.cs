using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface IKhuyenMaiServices
    {
        public bool Add(KhuyenMaiView kmv);
        public bool AdKMVoBT(List<Guid> btrequest, Guid IdKhuyenMai);
        public bool Ad1KMVo1BT(Guid btrequest, Guid IdKhuyenMai);

        public bool Update(KhuyenMaiView kmv);
        public bool Delete(Guid Id);

        public KhuyenMai GetById(Guid Id);
        public List<KhuyenMai> GetAll();
        public List<BienThe> GetAllBienThe();
        public List<KhuyenMai> GetKMByName(string Ten);
    }
}
