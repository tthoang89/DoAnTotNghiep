using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienService nhanVienService;
        public NhanVienController(INhanVienService nhanVienService)
        {
            this.nhanVienService = nhanVienService;
        }
        // GET: api/<NhanVienController>
        [HttpGet("GetAll")]
        public List<NhanVien> GetAllNhanVien()
        {
            return nhanVienService.GetAll();
        }

        // GET api/<NhanVienController>/5
        [HttpGet("{id}")]
        public NhanVien GetNhanVienById(Guid id)
        {
            return nhanVienService.GetById(id);
        }

        // POST api/<NhanVienController>
        [HttpPost("DangKyNhanVien")]
        public async Task<IActionResult> DangKyNhanVien(string ten, string email, string sdt, string diachi, Guid idVaiTro, int trangthai, string password)
        { 
            nhanVienService.Add(ten,email, sdt, diachi,idVaiTro,trangthai,password);

            return Ok("Đăng ký thành công");
        }

        // PUT api/<NhanVienController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, string ten, string email, string sdt, string diachi, Guid idVaiTro, int trangthai, string password)
        {
            var nv = nhanVienService.GetById(id);
            if (nv != null)
            {
                nv.Ten = ten;
                nv.Email = email;
                nv.PassWord = password;
                nv.SDT = sdt;
                nv.DiaChi   = diachi;
                nv.TrangThai = trangthai;
                nv.IDVaiTro = idVaiTro;
                return nhanVienService.Update(nv);
            }
            return false;
        }

        // DELETE api/<NhanVienController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var nv = nhanVienService.GetById(id);
            if(nv != null)
            {
                return nhanVienService.Delete(nv.ID);
            }
            return false;
        }
    }
}
