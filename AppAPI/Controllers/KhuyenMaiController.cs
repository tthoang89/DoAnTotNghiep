using AppAPI.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiServices _khuyenmai;
        public KhuyenMaiController(IKhuyenMaiServices khuyenmai)
        {
          this._khuyenmai = khuyenmai;
        }

        // GET: api/<KhuyenMaiController>
        [HttpGet]
        public List<KhuyenMai> Get()
        {
            return _khuyenmai.GetAll();
        }

        // GET api/<KhuyenMaiController>/5
        [HttpGet("{id}")]
        public KhuyenMai Get(Guid id)
        {
            return _khuyenmai.GetById(id);
        }

        // POST api/<KhuyenMaiController>
        [HttpPost]
        public bool Post(string ten, int giatri, DateTime NgayApDung, DateTime NgayKetThuc, string mota, int trangthai)
        {
            return _khuyenmai.Add(ten, giatri, NgayApDung, NgayKetThuc, mota, trangthai);
        }

        // PUT api/<KhuyenMaiController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, string ten, int giatri, DateTime NgayApDung, DateTime NgayKetThuc, string mota, int trangthai)
        {
            var khuyenmai= _khuyenmai.GetById(id);
            if (khuyenmai != null)
            {
                return _khuyenmai.Update(khuyenmai.ID,ten, giatri, NgayApDung, NgayKetThuc, mota, trangthai);
            }
            else
            {
                return false;
            }
        }

        // DELETE api/<KhuyenMaiController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var khuyenmai = _khuyenmai.GetById(id);
            if (khuyenmai != null)
            {
                return _khuyenmai.Delete(khuyenmai.ID);
            }
            else
            {
                return false;
            }
        }
    }
}
