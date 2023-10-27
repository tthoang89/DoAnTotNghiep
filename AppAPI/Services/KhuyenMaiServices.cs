using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class KhuyenMaiServices : IKhuyenMaiServices
    {
        private readonly IAllRepository<KhuyenMai> _repos;
        private readonly IAllRepository<BienThe> _reposbienthe;
        AssignmentDBContext context= new AssignmentDBContext();
        public KhuyenMaiServices()
        {
            _repos= new AllRepository<KhuyenMai>(context,context.KhuyenMais);
            _reposbienthe= new AllRepository<BienThe>(context,context.BienThes);
        }

        public bool Ad1KMVo1BT(Guid btrequest, Guid IdKhuyenMai)
        {
            var idbt=_reposbienthe.GetAll().FirstOrDefault(x=>x.ID==btrequest);
            if (idbt != null)
            {
                var timkiem=_repos.GetAll().FirstOrDefault(x=>x.ID == IdKhuyenMai);
                if (timkiem != null)
                {
                    if (timkiem.NgayKetThuc < DateTime.Now)
                    {
                        return false;
                    }
                    else
                    {
                        idbt.IDKhuyenMai = IdKhuyenMai;
                       return _reposbienthe.Update(idbt);
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
           
        }

        public bool Add(KhuyenMaiView kmv)
        {
            kmv.ID = Guid.NewGuid();
           var khuyenmai= new KhuyenMai();
            khuyenmai.ID=kmv.ID;
            khuyenmai.Ten=kmv.Ten;
            khuyenmai.GiaTri=kmv.GiaTri;
            khuyenmai.MoTa = kmv.MoTa;
            khuyenmai.NgayApDung=kmv.NgayApDung;
            khuyenmai.NgayKetThuc =kmv. NgayKetThuc;
            if (khuyenmai.NgayApDung > khuyenmai.NgayKetThuc)
            {
                return false;
            }
            khuyenmai.TrangThai = kmv.TrangThai;
            return _repos.Add(khuyenmai);
        }

        public bool AdKMVoBT(List<Guid> btrequest, Guid IdKhuyenMai)
        {
            
            foreach (var km in btrequest)
            {
                var timidkm = _repos.GetAll().FirstOrDefault(x => x.ID == IdKhuyenMai);
                if (timidkm.NgayKetThuc < DateTime.Now)
                {
                    return false;
                }
                else
                {
                    var tim = _reposbienthe.GetAll().FirstOrDefault(x => x.ID == km);
                    if (tim != null)
                    {
                        tim.IDKhuyenMai = IdKhuyenMai;
                        _reposbienthe.Update(tim);
                    }
                }

            }
            return true;
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

        public List<BienThe> GetAllBienThe()
        {
            return _reposbienthe.GetAll();
        }

        public KhuyenMai GetById(Guid Id)
        {
            return _repos.GetAll().FirstOrDefault(x => x.ID == Id);
        }

        public List<KhuyenMai> GetKMByName(string Ten)
        {
            return _repos.GetAll().Where(x=>x.Ten.Contains(Ten)).ToList();
        }

        public bool Update(KhuyenMaiView kmv)
        {
            var khuyenmai= _repos.GetAll().FirstOrDefault(x => x.ID == kmv.ID);
            if(khuyenmai!=null)
            {
                khuyenmai.Ten = kmv.Ten;
                khuyenmai.GiaTri = kmv.GiaTri;
                khuyenmai.MoTa = kmv.MoTa;
                khuyenmai.NgayApDung = kmv.NgayApDung;
                khuyenmai.NgayKetThuc = kmv.NgayKetThuc;
                if (khuyenmai.NgayApDung > khuyenmai.NgayKetThuc)
                {
                    return false;
                }
                khuyenmai.TrangThai = kmv.TrangThai;
                return _repos.Update(khuyenmai);
            }
            else
            {
                return false;
            }
        }
    }
}
