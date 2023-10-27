using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;

namespace AppAPI.Services
{
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly IAllRepository<ChiTietHoaDon> reposChiTietHoaDon;
        AssignmentDBContext context = new AssignmentDBContext();

        public ChiTietHoaDonService()
        {
            reposChiTietHoaDon = new AllRepository<ChiTietHoaDon>(context, context.ChiTietHoaDons);
        }

        public bool CreateCTHoaDon(ChiTietHoaDon chiTietHoaDon)
        {
            ChiTietHoaDon chiTHD = new ChiTietHoaDon();
            chiTHD.ID = Guid.NewGuid();
            chiTHD.DonGia = chiTHD.DonGia;
            chiTHD.SoLuong = chiTHD.SoLuong;
            chiTHD.TrangThai = chiTHD.TrangThai;
            chiTHD.IDHoaDon = chiTHD.IDHoaDon;
            chiTHD.IDBienThe = chiTHD.IDBienThe;
            return reposChiTietHoaDon.Add(chiTHD);
        }

        public bool DeleteCTHoaDon(Guid id)
        {
            var CTHD = reposChiTietHoaDon.GetAll().FirstOrDefault(p=>p.ID == id);
            return reposChiTietHoaDon.Delete(CTHD);
        }

        public List<ChiTietHoaDon> GetAllCTHoaDon()
        {
            return reposChiTietHoaDon.GetAll();
        }

        public bool UpdateCTHoaDon(ChiTietHoaDon chiTietHoaDon)
        {
            var CTHD = reposChiTietHoaDon.GetAll().FirstOrDefault(p=>p.ID == chiTietHoaDon.ID);
            CTHD.DonGia = chiTietHoaDon.DonGia;
            CTHD.SoLuong = chiTietHoaDon.SoLuong;
            CTHD.TrangThai = chiTietHoaDon.TrangThai;
            CTHD.IDHoaDon = chiTietHoaDon.IDHoaDon;
            CTHD.IDBienThe = chiTietHoaDon.IDBienThe;
            return reposChiTietHoaDon.Update(CTHD);
        }
    }
}
