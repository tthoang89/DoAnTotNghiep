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
        [HttpGet("[action]")]
        public IEnumerable<NhanVien> SearchTheoTen(string name)
        {
            return nhanVienService.GetByName(name);


        }
        // GET api/<NhanVienController>/5
        [HttpGet("{id}")]
        public NhanVien? GetById(Guid id)
        {
            return nhanVienService.GetById(id);
        }


        // POST api/<NhanVienController>
        [HttpPost("DangKyNhanVien")]
        public bool Add(string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro)
        {
            if (nhanVienService.Add(ten, email, password, sdt, diachi, trangthai, idvaitro))
            {
                return true;
            }
            return false;
        }

        // PUT api/<NhanVienController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro)
        {
            if (nhanVienService.Update(id, ten, email, password, sdt, diachi, trangthai, idvaitro))
            {
                return true;
            }
            return false;
        }

        // DELETE api/<NhanVienController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            if (nhanVienService.Delete(id))
            {
                return true;
            }
            return false;
        }

    }
}
