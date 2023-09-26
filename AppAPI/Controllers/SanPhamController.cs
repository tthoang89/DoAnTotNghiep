using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/SanPham")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamService _sanPhamServices;
        public SanPhamController()
        {
            this._sanPhamServices = new SanPhamService();
        }
        #region SanPham

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllSanPham()
        {
            var listSP = await _sanPhamServices.GetAllSanPham();
            return Ok(listSP);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetSanPhamById(Guid id)
        {
            var sanPham = await _sanPhamServices.GetSanPhamById(id);
            if(sanPham == null) return NotFound();
            return Ok(sanPham);
        }
        //[HttpGet("timKiemNC")]
        //public async Task<IActionResult> TimKiemSanPham(SanPhamTimKiemNangCao sp)
        //{
        //    var listSP = await _sanPhamServices.TimKiemSanPham(sp);
        //    return Ok(listSP);
        //}
        [HttpPost("save")]
        public async Task<IActionResult> CreateOrUpdateSanPham(SanPhamRequest request)
        {
            if (request == null) return BadRequest();
            var sanPham = await _sanPhamServices.SaveSanPham(request);
            return Ok(sanPham);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSanPham(Guid id)
        {
            var sanPham = await _sanPhamServices.DeleteSanPham(id);
            return Ok();
        }
        #endregion
        
    }
}
