using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyNguoiDungController : ControllerBase
    {
        private IQuanLyNguoiDungService _service;
        private List<KhachHang> listKH;
        private List<NhanVien> listNV;

        public QuanLyNguoiDungController(IQuanLyNguoiDungService service, List<KhachHang> listKH, List<NhanVien> listNV)
        {
            this._service = service;
            this.listKH = listKH;
            this.listNV = listNV;
        }




        // POST api/<DangNhapController>
        [HttpPost("DangNhap")]
        public IActionResult Post(string email, string password)
        {
            bool result = _service.DangNhap(email, password);
            if(result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // POST api/<DangKyController>
        [HttpPost("DangKyKhachHang")]
        public IActionResult DangKyKhachHang(string ten, DateTime ngaysinh, string password, int gioitinh, string email, int trangThai, string diachi, string sdt, int diemtich)
        {
            var khachHang = new KhachHang
            {
                IDKhachHang = Guid.NewGuid(),
                Ten = ten,
                NgaySinh = ngaysinh,
                Password = password,
                GioiTinh = gioitinh,
                Email = email,
                TrangThai = trangThai,
                DiaChi = diachi,
                SDT = sdt,
                DiemTich = diemtich,
            };
                listKH.Add(khachHang);
                return Ok("Đăng ký thành công");    
        }
        [HttpPost("DangKyNhanVien")]
        public IActionResult DangKyNhanVien(string ten, string email,string password, string sdt, string diachi, Guid idVaiTro, int trangthai)
        {
            var nv = new NhanVien
            {
                ID = Guid.NewGuid(),
                Ten = ten,
                Email = email,
                PassWord = password,
                SDT = sdt,
                DiaChi = diachi,
                TrangThai = trangthai,
                IDVaiTro = idVaiTro
            };
            listNV.Add(nv);
            return Ok("Đăng ký thành công");
        }
        [HttpPost("DoiMatKhauKhachHang")]
        public IActionResult DoiMatKhauKH(string email, string oldPassword,string newPassword)
        {
            bool result = _service.DoiMatKhauKH(email, oldPassword, newPassword);
            if (result)
            {
                return Ok("Đổi mật khẩu khách hàng thành công");
            }
            else
            {
                return BadRequest("Đổi mật khẩu khách hàng không thành công");
            }
        }
        [HttpPost("DoiMatKhauNhanViem")]
        public IActionResult DoiMatKhauNV(string email, string oldPassword, string newPassword)
        {
            bool result = _service.DoiMatKhauNV(email, oldPassword, newPassword);
            if (result)
            {
                return Ok("Đổi mật khẩu nhân viên thành công");
            }
            else
            {
                return BadRequest("Đổi mật khẩu nhân viên không thành công");
            }
        }
    }
}
