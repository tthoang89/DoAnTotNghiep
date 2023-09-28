using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;

namespace AppAPI.Services
{
    public class KhuyenMaiServices : IKhuyenMaiServices
    {
        private readonly IAllRepository<KhuyenMai> _repos;
        AssignmentDBContext context= new AssignmentDBContext();
        public KhuyenMaiServices()
        {
            _repos= new AllRepository<KhuyenMai>(context,context.KhuyenMais);
        }
        public bool Add(string ten, int giatri, DateTime NgayApDung, DateTime NgayKetThuc, string mota, int trangthai)
        {
           var khuyenmai= new KhuyenMai();
            khuyenmai.ID=Guid.NewGuid();
            khuyenmai.Ten=ten;
            khuyenmai.GiaTri=giatri;
            khuyenmai.MoTa = mota;
            khuyenmai.NgayApDung=NgayApDung;
            khuyenmai.NgayKetThuc = NgayKetThuc;
            if (khuyenmai.NgayApDung > khuyenmai.NgayKetThuc)
            {
                return false;
            }
            khuyenmai.TrangThai = trangthai;
            return _repos.Add(khuyenmai);
        }

        public bool Delete(Guid Id)
        {
            var khuyenmai = _repos.GetAll().FirstOrDefault(x => x.ID == Id);
            if (khuyenmai != null)
            {
               
                return _repos.Delete(khuyenmai);
            }
            else
            {
                return false;
            }
        }

        public List<KhuyenMai> GetAll()
        {
           return _repos.GetAll();
        }

        public KhuyenMai GetById(Guid Id)
        {
            return _repos.GetAll().FirstOrDefault(x => x.ID == Id);
        }

        public bool Update(Guid Id, string ten, int giatri, DateTime NgayApDung, DateTime NgayKetThuc, string mota, int trangthai)
        {
            var khuyenmai= _repos.GetAll().FirstOrDefault(x => x.ID == Id);
            if(khuyenmai!=null)
            {
                khuyenmai.Ten = ten;
                khuyenmai.GiaTri = giatri;
                khuyenmai.MoTa = mota;
                khuyenmai.NgayApDung = NgayApDung;
                khuyenmai.NgayKetThuc = NgayKetThuc;
                if (khuyenmai.NgayApDung > khuyenmai.NgayKetThuc)
                {
                    return false;
                }
                khuyenmai.TrangThai = trangthai;
                return _repos.Update(khuyenmai);
            }
            else
            {
                return false;
            }
        }
    }
}
