using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;

namespace AppAPI.Services
{
    public class NhanVienService : INhanVienService
    {
        private readonly AssignmentDBContext _dbContext;
        public NhanVienService(AssignmentDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Add(NhanVien nv)
        {
            try
            {
                _dbContext.NhanViens.Add(nv);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
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
           return _dbContext.NhanViens.ToList();
        }

        public NhanVien GetById(Guid id)
        {
            return _dbContext.NhanViens.FirstOrDefault(x => x.ID == id);
        }

        public bool Update(NhanVien nv)
        {
            try
            {
                var kh = _dbContext.NhanViens.FirstOrDefault(x => x.ID == nv.ID);
                if (kh != null)
                {
                    _dbContext.NhanViens.Update(kh);
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
    }
}
