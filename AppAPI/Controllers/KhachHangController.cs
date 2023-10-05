using AppAPI.IServices;
using AppAPI.Services;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
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
        [HttpPost("add")]
        public async Task<IActionResult> Pots(KhachHangViewModel model)
        {
            if (model == null) return BadRequest();
            var kh = await _khachHangService.Add(model);
            return Ok(kh);
        }

        // PUT api/<SanPhamController>/5
        [HttpPut("{id}")]
        public  bool Put(Guid id, KhachHangViewModel model)
        {
            var result = _khachHangService.Update(id, model.Email, model.Password);
           if(!result) return false;
           return true;
            
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
