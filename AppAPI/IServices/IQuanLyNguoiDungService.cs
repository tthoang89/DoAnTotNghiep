using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface IQuanLyNguoiDungService
    {
        Task<LoginViewModel> Login(string lg, string password);
        Task<KhachHang> RegisterKhachHang(KhachHangViewModel khachHang);
        Task<NhanVien> RegisterNhanVien(NhanVienViewModel nhanVien);
        Task<bool> ChangePassword(string email, string password, string newPassword);

    }
}
