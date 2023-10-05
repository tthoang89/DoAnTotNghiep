using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangService _khachHangService;
        public KhachHangController(IKhachHangService khachHangService)
        {
            this._khachHangService = khachHangService;
        }
        // GET: api/<SanPhamController>
        [HttpGet]
        public List<KhachHang> GetAllKhachHang()
        {
            return _khachHangService.GetAll();
        }

        // GET api/<SanPhamController>/5
        [HttpGet("{id}")]
        public KhachHang GetKhachHangById(Guid id)
        {
            var kh = _khachHangService.GetById(id);
            return kh;  
        }

        // POST api/<SanPhamController>
        [HttpPost]
        public bool Post(string ten, DateTime ngaysinh, string password, int gioitinh, string email, int trangThai, string diachi, string sdt, int diemtich, Guid idvaitro)
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
                DiemTich = diemtich
            };
            return _khachHangService.Add(khachHang);

        }   

        // PUT api/<SanPhamController>/5
        [HttpPut("{id}")]
        public  bool Put(Guid id, string ten, DateTime ngaysinh, string password, int gioitinh, string email, int trangThai, string diachi, string sdt, int diemtich, Guid idvaitro)
        {
            var result = _khachHangService.GetById(id);
            if (result == null)
            {
                return false;
            }
            result.Ten = ten;
            result.NgaySinh = ngaysinh;
            result.Password = password;
            result.Email = email;
            result.GioiTinh= gioitinh;
            result.TrangThai    = trangThai;
            result.DiaChi = diachi;
            result.SDT = sdt;
            result.DiemTich = diemtich;
            result.IDVaiTro= idvaitro;
            return _khachHangService.Update(result);
        }

        // DELETE api/<SanPhamController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var result = _khachHangService.Delete(id);
            return result;
        }
    }
}
