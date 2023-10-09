using AppAPI.IServices;
using AppAPI.Services;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security;

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
        public async Task<IActionResult> Post(KhachHangViewModel khachHang)
        {
            var kh = await _khachHangService.Add(khachHang);
            if (kh == null)
            {
                return BadRequest();
            }

            return Ok("Đăng ký thành công");
        }
        // PUT api/<SanPhamController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, string ten, string email, string password, string diachi, DateTime ngaysinh, int gioitinh, int diemtisch, int trnagthai, string sdt)
        {
            var nv = _khachHangService.GetById(id);
            if (nv != null)
            {
                nv.Ten = ten;
                nv.NgaySinh = ngaysinh;
                nv.DiaChi = diachi;
                nv.TrangThai = trnagthai;
                nv.SDT = sdt;
                nv.Email = email;
                nv.Password = password;
                nv.GioiTinh = gioitinh;
                nv.DiemTich = diemtisch;
                return _khachHangService.Update(nv);

            }
            return false;

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
