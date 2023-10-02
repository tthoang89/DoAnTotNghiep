using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface IQuanLyNguoiDungService
    {
        Task<bool> Login(string email, string password);
        Task<List<NhanVien>> RegisterNhanVien(NhanVienViewmodel nhanVien);
        Task<KhachHang> RegisterKhachHang(KhachHangViewModel khachHang);
        Task<bool> ChangePasswordNhanVien(string email, string password, string newPassword);
        Task<bool> ChangePasswordKhachHang(string email, string password, string newPassword);
    }
}
