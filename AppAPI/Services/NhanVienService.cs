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

        public bool Delete(Guid id)
        {
            var nv = _dbContext.NhanViens.FirstOrDefault(nv => nv.ID == id);
            if (nv != null)
            {
                _dbContext.NhanViens.Remove(nv);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<NhanVien> GetAll()
        {
            return _dbContext.NhanViens.ToList();
        }

        public IEnumerable<NhanVien> GetByName(string name)
        {
            var nv = _dbContext.NhanViens.Where(x => x.Ten.ToLower().StartsWith(name.ToLower()));
            return nv;

        }

        public bool Update(Guid id, string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro)
        {
            var nv = _dbContext.NhanViens.FirstOrDefault(x => x.ID == id);
            if (nv != null)
            {
                nv.Ten = ten;
                nv.Email = email;
                nv.PassWord = password;
                nv.SDT = sdt;
                nv.DiaChi = diachi;
                nv.TrangThai = trangthai;
                nv.IDVaiTro = idvaitro;
                _dbContext.NhanViens.Update(nv);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Add(string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro)
        {

            try
            {
                NhanVien nv = new NhanVien();
                nv.ID = Guid.NewGuid();
                nv.Ten = ten;
                nv.Email = email;
                nv.PassWord = password;
                nv.SDT = sdt;
                nv.DiaChi = diachi;
                nv.TrangThai = trangthai;
                nv.IDVaiTro =idvaitro;
                _dbContext.NhanViens.Add(nv);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public NhanVien? GetById(Guid id)
        {
            var nv = _dbContext.NhanViens.FirstOrDefault(nv => nv.ID == id);
            return nv;
        }
    }
}
