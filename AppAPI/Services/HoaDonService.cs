using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;

namespace AppAPI.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IAllRepository<HoaDon> reposHoaDon;
        private readonly IAllRepository<ChiTietHoaDon> reposChiTietHoaDon;
        private readonly IAllRepository<BienThe> reposBienThe;
        AssignmentDBContext context = new AssignmentDBContext();

        public HoaDonService()
        {
            reposHoaDon = new AllRepository<HoaDon>(context, context.HoaDons);
            reposChiTietHoaDon = new AllRepository<ChiTietHoaDon>(context, context.ChiTietHoaDons);
            reposBienThe = new AllRepository<BienThe>(context, context.BienThes);
        }

        public bool CreateHoaDon(List<ChiTietHoaDonViewModel> chiTietHoaDons, HoaDonViewModel hoaDon)
        {
            try
            {
                HoaDon hoaDon1 = new HoaDon();
                hoaDon1.ID = Guid.NewGuid();
                hoaDon1.IDNhanVien = hoaDon.IDNhanVien;
                hoaDon1.IDVoucher = hoaDon.IDVoucher;
                hoaDon1.TenNguoiNhan = hoaDon.Ten;
                hoaDon1.SDT = hoaDon.SDT;
                hoaDon1.Email = hoaDon.Email;
                hoaDon1.NgayTao = DateTime.Now;
                hoaDon1.DiaChi = hoaDon.DiaChi;
                hoaDon1.TienShip = hoaDon.TienShip;
                hoaDon1.PhuongThucThanhToan = hoaDon.PhuongThucThanhToan;
                hoaDon1.TrangThaiGiaoHang = 1;
                if (reposHoaDon.Add(hoaDon1))
                {
                    if (chiTietHoaDons != null)
                    {
                        foreach (var x in chiTietHoaDons)
                        {
                            ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
                            chiTietHoaDon.ID = Guid.NewGuid();
                            chiTietHoaDon.IDHoaDon = hoaDon1.ID;
                            chiTietHoaDon.IDBienThe = x.IDBienThe;
                            chiTietHoaDon.SoLuong = x.SoLuong;
                            chiTietHoaDon.DonGia = reposBienThe.GetAll().First(y => y.ID == x.IDBienThe).GiaBan;
                            chiTietHoaDon.TrangThai = 1;
                            reposChiTietHoaDon.Add(chiTietHoaDon);
                            var bienThe = reposBienThe.GetAll().FirstOrDefault(p=>p.ID == x.IDBienThe);
                            bienThe.SoLuong -= chiTietHoaDon.SoLuong;
                            reposBienThe.Update(bienThe);
                        }
                        return true;
                    }
                    else
                    {
                        reposHoaDon.Delete(hoaDon1);
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ChiTietHoaDon> GetAllChiTietHoaDon(Guid idHoaDon)
        {
            return reposChiTietHoaDon.GetAll().Where(x => x.IDHoaDon == idHoaDon).ToList();
        }

        public List<HoaDon> GetAllHoaDon()
        {
            return reposHoaDon.GetAll();
        }

        public bool UpdateTrangThaiGiaoHang(Guid idHoaDon, int trangThai)
        {
            var update = reposHoaDon.GetAll().FirstOrDefault(p=>p.ID == idHoaDon);
            if (update != null)
            {
                update.TrangThaiGiaoHang = trangThai;
                reposHoaDon.Update(update);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
