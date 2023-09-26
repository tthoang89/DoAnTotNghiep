using AppData.Models;

namespace AppAPI.IServices
{
    public interface IQuanLyNguoiDungService
    {
        public bool DangNhap(string email, string password);
        public bool DangKyKhachHang(KhachHang khachHang);
        public bool DangKyNhanVien(NhanVien nhanVien);
        public bool DoiMatKhauNV(string email, string oldPassword, string newPassword);
        public bool DoiMatKhauKH(string email, string oldPassword, string newPassword);
    }
}
