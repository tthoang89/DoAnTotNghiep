using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;

namespace AppAPI.Services
{
    public class ChiTietGioHangServices : IChiTietGioHangServices
    {
        private readonly IAllRepository<ChiTietGioHang> repos;
        private readonly IAllRepository<BienThe> bienthes;
        AssignmentDBContext context = new AssignmentDBContext();
        public ChiTietGioHangServices()
        {
            repos = new AllRepository<ChiTietGioHang>(context, context.ChiTietGioHangs);
            bienthes = new AllRepository<BienThe>(context, context.BienThes);
        }
        public string Add(Guid IdBienThe, Guid IdKhachHang, int soluong)
        {
            ChiTietGioHang chiTietGioHang = new ChiTietGioHang();
            chiTietGioHang.ID = Guid.NewGuid();
            chiTietGioHang.IDBienThe = IdBienThe;
            chiTietGioHang.IDNguoiDung = IdKhachHang;
            chiTietGioHang.SoLuong = soluong;
            if (repos.GetAll().Exists(p => p.IDBienThe == IdBienThe && p.IDNguoiDung == IdKhachHang))
            {
                Guid id = repos.GetAll().Find(p => p.IDBienThe == IdBienThe && p.IDNguoiDung == IdKhachHang).ID;
                ChiTietGioHang chiTietGioHang1 = repos.GetAll().Find(p => p.IDBienThe == IdBienThe && p.IDNguoiDung == IdKhachHang);
                if (chiTietGioHang.SoLuong + soluong > bienthes.GetAll().Find(p => p.ID == IdBienThe).SoLuong)
                {
                    return "so luong trong kho khong du";
                }
                else
                {
                    chiTietGioHang1.SoLuong = chiTietGioHang1.SoLuong + soluong;
                    return repos.Update(chiTietGioHang1).ToString();
                }
            }
            else
            {
                return repos.Add(chiTietGioHang).ToString();
            }
        }

        public bool Delete(Guid Id)
        {
            var chiTietGioHang = repos.GetAll().FirstOrDefault(x => x.ID == Id);
            if (chiTietGioHang != null)
            {

                return repos.Delete(chiTietGioHang);
            }
            else
            {
                return false;
            }
        }

        public List<ChiTietGioHang> GetAll()
        {
            return repos.GetAll().ToList();
        }

        public ChiTietGioHang GetById(Guid Id)
        {
            return repos.GetAll().FirstOrDefault(x => x.ID == Id);
        }

        public string Update(Guid id, Guid IdBienThe, Guid IdKhachHang, int soluong)
        {
            var chiTietGioHang = repos.GetAll().FirstOrDefault(x => x.ID == id);
            if (chiTietGioHang != null)
            {
                chiTietGioHang.IDBienThe = IdBienThe;
                chiTietGioHang.IDNguoiDung = IdKhachHang;
                chiTietGioHang.SoLuong = soluong;
                if (repos.GetAll().Exists(x => x.IDBienThe == IdBienThe && x.IDNguoiDung == IdKhachHang))
                {

                    ChiTietGioHang chiTietGioHang1 = repos.GetAll().Find(p => p.IDBienThe == IdBienThe && p.IDNguoiDung == IdKhachHang);
                    if (chiTietGioHang1.SoLuong > bienthes.GetAll().Find(x => x.ID == IdBienThe).SoLuong)
                    {
                        return "so luong trong kho khong du";
                    }
                    else
                    {
                        chiTietGioHang1.SoLuong = soluong;
                        return repos.Update(chiTietGioHang1).ToString();
                    }
                }
                else
                {
                    return repos.Update(chiTietGioHang).ToString();
                }
            }
            else
            {
                return "";
            }
        }
    }
}
