using AppData.Models;

namespace AppAPI.IServices
{
    public interface IKhuyenMaiServices
    {
        public bool Add(string ten,int giatri,DateTime NgayApDung,DateTime NgayKetThuc,string mota,int trangthai);
        public bool Update(Guid Id, string ten, int giatri, DateTime NgayApDung, DateTime NgayKetThuc, string mota, int trangthai);
        public bool Delete(Guid Id);
        public KhuyenMai GetById(Guid Id);
        public List<KhuyenMai> GetAll();
    }
}
