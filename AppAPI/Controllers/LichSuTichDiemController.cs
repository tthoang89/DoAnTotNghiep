using AppAPI.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichSuTichDiemController : ControllerBase
    {
        private readonly ILishSuTichDiemServices _lichsu;
        public LichSuTichDiemController(ILishSuTichDiemServices lishSuTichDiemServices)
        {
            this._lichsu = lishSuTichDiemServices;
        }
        // GET: api/<LichSuTichDiemController>
        [HttpGet]
        public List<LichSuTichDiem> Get()
        {
            return _lichsu.GetAll();
        }

        // GET api/<LichSuTichDiemController>/5
        [HttpGet("{id}")]
        public LichSuTichDiem Get(Guid id)
        {
            return _lichsu.GetById(id);
        }

        // POST api/<LichSuTichDiemController>
        [HttpPost]
        public bool Post(int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            return _lichsu.Add(diem, trangthai, IdKhachHang, IdQuyDoiDiem, IdHoaDon);
        }

        // PUT api/<LichSuTichDiemController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            var lichsu= _lichsu.GetById(id);
            if (lichsu != null)
            {
                return _lichsu.Update(lichsu.ID,diem, trangthai, IdKhachHang, IdQuyDoiDiem, IdHoaDon);
            }
            else
            {
                return false;
            }
        }

        // DELETE api/<LichSuTichDiemController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var lichsu = _lichsu.GetById(id);
            if (lichsu != null)
            {
                return _lichsu.Delete(lichsu.ID);
            }
            else
            {
                return false;
            }
        }
    }
}
