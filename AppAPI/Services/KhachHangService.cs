using AppAPI.IServices;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly AssignmentDBContext _dbContext;
        public KhachHangService(AssignmentDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(KhachHang nv)
        {
            try
            {
                _dbContext.KhachHangs.Add(nv);
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
                var kh = _dbContext.KhachHangs.FirstOrDefault(x => x.IDKhachHang == id);
                if (kh != null)
                {
                    _dbContext.KhachHangs.Remove(kh);
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

        public List<KhachHang> GetAll()
        {
            return _dbContext.KhachHangs.ToList();
        }

        public KhachHang GetById(Guid id)
        {
            return _dbContext.KhachHangs.FirstOrDefault(x => x.IDKhachHang == id);

        }

        public bool Update(KhachHang nv)
        {
            try
            {
                var kh = _dbContext.KhachHangs.FirstOrDefault(x => x.IDKhachHang == nv.IDKhachHang);
                if (kh != null)
                {
                    _dbContext.KhachHangs.Update(kh);
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
