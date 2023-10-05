using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface IQuanLyNguoiDungService
    {
        Task<bool> Login(string email, string password);
        Task<KhachHang> RegisterKhachHang(KhachHangViewModel khachHang);
        Task<bool> ChangePassword(string email, string password, string newPassword);
       
    }
}
