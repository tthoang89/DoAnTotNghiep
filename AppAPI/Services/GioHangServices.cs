using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;

namespace AppAPI.Services
{
    public class GioHangServices : IGioHangServices
    {
        private readonly IAllRepository<GioHang> repos;
        AssignmentDBContext context = new AssignmentDBContext();
        private readonly ISanPhamService _iSanPhamService;
        public GioHangServices()
        {
            repos = new AllRepository<GioHang>(context, context.GioHangs);
            _iSanPhamService = new SanPhamService(context);
        }
        public bool Add(Guid IdKhachHang, DateTime ngaytao)
        {
            GioHang gioHang = new GioHang();
            gioHang.IDKhachHang = IdKhachHang;
            gioHang.NgayTao = ngaytao;
            return repos.Add(gioHang);
        }

        public bool Delete(Guid Id)
        {
            var gioHang = repos.GetAll().FirstOrDefault(x => x.IDKhachHang == Id);
            if (gioHang != null)
            {
                return repos.Delete(gioHang);
            }
            else
            {
                return false;
            }
        }

        public List<GioHang> GetAll()
        {
            return repos.GetAll().ToList();
        }

        public GioHang GetById(Guid Id)
        {
            return repos.GetAll().FirstOrDefault(x => x.IDKhachHang == Id);
        }

        public bool Update(Guid Id, DateTime ngaytao)
        {
            var gioHang = repos.GetAll().FirstOrDefault(x => x.IDKhachHang == Id);
            if (gioHang != null)
            {
                gioHang.NgayTao = ngaytao;
                return repos.Update(gioHang);
            }
            else
            {
                return false;
            }
        }
        public GioHangViewModel GetCart(List<GioHangRequest> request)
        {
            var response = new GioHangViewModel();
            long tongTien = 0;
            ChiTietSanPhamViewModel chiTietSanPham;
            foreach (var item in request)
            {
                chiTietSanPham = _iSanPhamService.GetChiTietSanPhamByID(item.IDChiTietSanPham);
                item.DonGia = chiTietSanPham.GiaBan;
                item.Ten = chiTietSanPham.Ten;
                item.MauSac = chiTietSanPham.MauSac;
                item.KichCo = chiTietSanPham.KichCo;
                item.Anh = chiTietSanPham.Anh;
                tongTien += item.DonGia.Value * item.SoLuong;
            }
            response.GioHangs = request;
            response.TongTien = tongTien;
            return response;
        }
    }
}
