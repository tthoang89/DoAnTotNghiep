using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;

namespace AppAPI.Services
{
    public class LishSuTichDiemServices : ILishSuTichDiemServices
    {
        private readonly IAllRepository<LichSuTichDiem> _allRepository;
        AssignmentDBContext context= new AssignmentDBContext();
        public LishSuTichDiemServices()
        {
            _allRepository= new AllRepository<LichSuTichDiem>(context,context.LichSuTichDiems);
        }
        public bool Add(int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            var lichsu = new LichSuTichDiem();
            lichsu.ID=Guid.NewGuid();
            lichsu.Diem=diem;
            lichsu.TrangThai=trangthai;
            lichsu.IDKhachHang=IdKhachHang;
            lichsu.IDQuyDoiDiem = IdQuyDoiDiem;
            lichsu.IDHoaDon = IdHoaDon;
            return _allRepository.Add(lichsu);
        }

        public bool Delete(Guid Id)
        {
            var lichsu = _allRepository.GetAll().First(x => x.ID == Id);
            if (lichsu != null)
            {
               
                return _allRepository.Delete(lichsu);
            }
            else
            {
                return false;
            }
        }

        public List<LichSuTichDiem> GetAll()
        {
            return _allRepository.GetAll(); 
        }

        public LichSuTichDiem GetById(Guid Id)
        {
            return _allRepository.GetAll().First(x => x.ID==Id);
        }

        public bool Update(Guid Id, int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            var lichsu= _allRepository.GetAll().First(x => x.ID == Id);
            if (lichsu != null)
            {
                lichsu.Diem = diem;
                lichsu.TrangThai = trangthai;
                lichsu.IDKhachHang = IdKhachHang;
                lichsu.IDQuyDoiDiem = IdQuyDoiDiem;
                lichsu.IDHoaDon = IdHoaDon;
                return _allRepository.Update(lichsu);
            }
            else
            {
                return false;
            }
        }
    }
}
