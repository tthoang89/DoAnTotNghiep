using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly IAllRepository<NguoiDung> repos;
        AssignmentDBContext context = new AssignmentDBContext();
        public NguoiDungController()
        {
            repos = new AllRepository<NguoiDung>(context, context.NguoiDungs);
        }
        // GET: api/<SanPhamController>
        [HttpGet]
        public List<NguoiDung> Get()
        {
            return repos.GetAll();
        }

        // GET api/<SanPhamController>/5
        [HttpGet("{ten}")]
        public List<NguoiDung> Get(string name)
        {
            return repos.GetAll().Where(x => x.Ten.Contains(name)).ToList();
        }

        // POST api/<SanPhamController>
        [HttpPost("create")]
        public bool Post(string ten, string tendem, string ho, DateTime ngaysinh,string password, int gioitinh, string email, string diachi, string sdt, int diemtich, Guid idvaitro)
        {
            NguoiDung nguoiDung = new NguoiDung();
            nguoiDung.IDNguoiDung = Guid.NewGuid();
            nguoiDung.Ho = ho;
            nguoiDung.Ten = ten;
            nguoiDung.TenDem = tendem;
            nguoiDung.GioiTinh = gioitinh;
            nguoiDung.NgaySinh = ngaysinh;
            nguoiDung.Password = password;
            nguoiDung.Email = email;
            nguoiDung.DiaChi = diachi;
            nguoiDung.SDT = sdt;
            nguoiDung.DiemTich = diemtich;
            nguoiDung.TrangThai = 1;
            nguoiDung.IDVaiTro = idvaitro;
            return repos.Add(nguoiDung);
        }

        // PUT api/<SanPhamController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, string ten, string tendem, string ho, DateTime ngaysinh,string password, int gioitinh, string email, string diachi, string sdt, int diemtich, Guid idvaitro)
        {
            var nguoiDung = repos.GetAll().FirstOrDefault(x => x.IDNguoiDung == id);
            if (nguoiDung != null)
            {
                nguoiDung.Ho = ho;
                nguoiDung.Ten = ten;
                nguoiDung.TenDem = tendem;
                nguoiDung.GioiTinh = gioitinh;
                nguoiDung.NgaySinh = ngaysinh;
                nguoiDung.Password = password;
                nguoiDung.Email = email;
                nguoiDung.DiaChi = diachi;
                nguoiDung.SDT = sdt;
                nguoiDung.DiemTich = diemtich;
                nguoiDung.TrangThai = 1;
                nguoiDung.IDVaiTro = idvaitro;
                return repos.Update(nguoiDung);
            }
            else
            {
                return false;
            }
        }

        // DELETE api/<SanPhamController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var nguoiDung = repos.GetAll().FirstOrDefault(x => x.IDNguoiDung == id);
            return repos.Delete(nguoiDung);
        }
    }
}
