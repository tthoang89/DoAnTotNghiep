//using AppAPI.IServices;
//using AppData.Models;

//namespace AppAPI.Services
//{
//    public class QuanLyNguoiDungService : IQuanLyNguoiDungService
//    {
//        private List<KhachHang> listKhachHang;
//        private List<NhanVien> listNhanVien;

//        public QuanLyNguoiDungService(List<KhachHang> listKhachHang, List<NhanVien> listNhanVien)
//        {
//            this.listKhachHang = listKhachHang;
//            this.listNhanVien = listNhanVien;
//        }
//        public bool DangKyKhachHang(KhachHang khachHang)
//        {
//            bool khDaCo = listKhachHang.Any( x=> x.Email == khachHang.Email);
//            if (!khDaCo)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public bool DangKyNhanVien(NhanVien nhanVien)
//        {
//            bool nvDaCo = listNhanVien.Any(a=> a.Email == nhanVien.Email);
//            if (!nvDaCo)
//            {
//                return true;
//            }
//            else
//            { 
//                return false; 
//            }
//        }

//        public bool DangNhap(string email, string password)
//        {
//            KhachHang kh = listKhachHang.FirstOrDefault(x=>x.Email == email && x.Password == password); 
//            if (kh != null)
//            {
//                return true;
//            }
//            NhanVien nhanVien = listNhanVien.FirstOrDefault(n=>n.Email == email && n.PassWord == password);
//            if (nhanVien != null)
//            {
//                return true;
//            }
//            return false;
//        }

//        public bool DoiMatKhauKH(string email, string oldPassword, string newPassword)
//        {
//            KhachHang khachHang = listKhachHang.FirstOrDefault(x => x.Email == email && x.Password == oldPassword);
//            if (khachHang != null)
//            {
//                khachHang.Password = newPassword;
//                return true;
//            }
//            return false;
//        }

//        public bool DoiMatKhauNV(string email, string oldPassword, string newPassword)
//        {
//            NhanVien nhanVien = listNhanVien.FirstOrDefault(n=>n.Email == email && n.PassWord==oldPassword);
//            if (nhanVien != null)
//            {
//                nhanVien.PassWord = newPassword;
//                return true;
//            }
//            return false;
//        }
//    }
//}
