using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class QuanLyNguoiDungService : IQuanLyNguoiDungService
    {
        private readonly IAllRepository<NhanVien> reposNV;
        private readonly IAllRepository<KhachHang> reposKH;
        AssignmentDBContext context = new AssignmentDBContext();

        public QuanLyNguoiDungService()
        {
            reposNV = new AllRepository<NhanVien>(context, context.NhanViens);
            reposKH = new AllRepository<KhachHang>(context, context.KhachHangs);

        }

        public async Task<bool> ChangePassword(string email, string password, string newPassword)
        {
            var kh = await context.KhachHangs.FirstOrDefaultAsync(h => h.Email == email && h.Password == password);
            if (kh != null)
            {
                kh.Password = newPassword;
                await context.SaveChangesAsync();
                return true;
            }
            var nv = await context.NhanViens.FirstOrDefaultAsync(h => h.Email == email && h.PassWord == password);
            if (nv != null)
            {
                nv.PassWord = newPassword;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<LoginViewModel> Login(string lg, string password)
        {
            var nv = await  context.NhanViens.FirstOrDefaultAsync(a => (a.Email == lg || a.SDT == lg) && a.PassWord == password);
            if(nv != null)
            {
                return new LoginViewModel 
                { 
                    Id = nv.ID,
                    Email = nv.Email, 
                    Ten = nv.Ten,
                    SDT = nv.SDT,
                    vaiTro = 0
                };
            }
            var kh = await context.KhachHangs.FirstOrDefaultAsync(x => (x.Email == lg || x.SDT == lg) && x.Password == password);
            if(kh != null)
            {
                return new LoginViewModel
                {
                    Id = kh.IDKhachHang,
                    Email = kh.Email,
                    Ten = kh.Ten,
                    SDT = kh.SDT,
                    DiemTich = kh.DiemTich,
                    vaiTro = 1
                };
            }
            return null;
        }

        //public async Task<object> Login(string email, string password)
        //{
        //    var nv = await context.NhanViens.FirstOrDefaultAsync(a => a.Email == email && a.PassWord == password);
        //    if (nv != null)
        //    {
        //        return nv;
        //    }
        //    var kh = await context.KhachHangs.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        //    if (kh != null)
        //    {
        //        return kh;
        //    }
        //    return null;
        //}

        public async Task<KhachHang> RegisterKhachHang(KhachHangViewModel khachHang)
        {
            KhachHang kh = new KhachHang()
            {
                IDKhachHang = Guid.NewGuid(),
                Ten = khachHang.Name,
                Email = khachHang.Email,
                Password = khachHang.Password
            };
            await context.KhachHangs.AddAsync(kh);
            //await context.SaveChangesAsync();
            GioHang gioHang = new GioHang()
            {
                IDKhachHang = kh.IDKhachHang,
                NgayTao = DateTime.Now,
            };
            await context.GioHangs.AddAsync(gioHang);
            await context.SaveChangesAsync();
            return kh;
        }

        public async Task<NhanVien> RegisterNhanVien(NhanVienViewModel nhanVien)
        {
            var kh = new NhanVien
            {
                ID = Guid.NewGuid(),
                Ten = nhanVien.Name,
                Email = nhanVien.Email,
                PassWord = nhanVien.Password,
                IDVaiTro = nhanVien.IDVaiTro
            };
            context.NhanViens.Add(kh);
            await context.SaveChangesAsync();
            return kh;
        }
        //Tam
        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            var kh = await context.KhachHangs.FirstOrDefaultAsync(h => h.IDKhachHang == request.ID);
            if (kh != null)
            {
                kh.Password = request.NewPassword;
                await context.SaveChangesAsync();
                return true;
            }
            var nv = await context.NhanViens.FirstOrDefaultAsync(h => h.ID == request.ID);
            if (nv != null)
            {
                nv.PassWord = request.NewPassword;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        //End
    }
}
