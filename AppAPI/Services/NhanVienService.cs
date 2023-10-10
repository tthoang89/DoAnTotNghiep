using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;

namespace AppAPI.Services
{
    public class NhanVienService : INhanVienService
    {
        private readonly 
        AssignmentDBContext _dbContext;

        public NhanVienService()
        {
            _dbContext = new AssignmentDBContext();
        }

        public bool Add(string ten, string email, string sdt, string diachi, Guid idVaiTro, int trangthai, string password)
        {
            var nv = new NhanVien();
            nv.ID = Guid.NewGuid();
            nv.Ten = ten;
            nv.Email = email;
            nv.PassWord = password;
            nv.DiaChi = diachi;
            nv.SDT = sdt;
            nv.IDVaiTro = idVaiTro;
            nv.TrangThai = trangthai;
            _dbContext.NhanViens.Add(nv);
            _dbContext.SaveChanges();
            return true;
            
        }

        public bool Delete(Guid id)
        {
            try
            {
                var kh = _dbContext.NhanViens.FirstOrDefault(x => x.ID == id);
                if (kh != null)
                {
                    _dbContext.NhanViens.Remove(kh);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<NhanVien> GetAll()
        {
            return _dbContext.NhanViens
                .ToList();
        }

        public NhanVien GetById(Guid id)
        {
            return _dbContext.NhanViens.FirstOrDefault(x => x.ID == id);
        }

        public bool Update(NhanVien nv)
        {
            var kh = _dbContext.NhanViens.FirstOrDefault(x => x.ID == nv.ID);
            if (kh != null)
            {
                kh.Ten = nv.Ten;
                kh.Email = nv.Email;
                kh.PassWord = nv.PassWord;
                kh.SDT = nv.SDT;
                kh.DiaChi = nv.DiaChi;
                kh.TrangThai = nv.TrangThai;
                kh.IDVaiTro = nv.IDVaiTro; _dbContext.NhanViens.Update(kh);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
