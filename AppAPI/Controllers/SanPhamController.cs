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
        [HttpGet("getByIdLsp/{idLsp}")]
        public async Task<IActionResult> GetSanPhamByIdDanhMuc(Guid idLsp)
        {
            var sanPham = await _sanPhamServices.GetSanPhamByIdDanhMuc(idLsp);
            if (sanPham == null) return NotFound();
            return Ok(sanPham);
        }
        [HttpGet("checkTrungTen")]
        public async Task<IActionResult> CheckTrung(SanPhamRequest request)
        {
            var listSP = _sanPhamServices.CheckTrungTenSP(request);
            return Ok(listSP);
        }
        [HttpPost("timKiemNC")]
        public async Task<IActionResult> TimKiemSanPham(SanPhamTimKiemNangCao sp)
        {
            var listSP = await _sanPhamServices.TimKiemSanPham(sp);
            return Ok(listSP);
        }
        [HttpPost("AddSanPham")]
        public async Task<IActionResult> CreateSanPham(SanPhamRequest request)
        {
            if (request == null) return BadRequest();
            var response = await _sanPhamServices.AddSanPham(request);
            return Ok(response);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSanPham(Guid id)
        {
            var sanPham = await _sanPhamServices.DeleteSanPham(id);
            return Ok();
        }
        #endregion

        #region ChiTietSanPham
        [HttpGet("GetAllChiTietSanPham")]
        public async Task<IActionResult> GetAllChiTietSanPham(Guid idSanPham)
        {
            var lstChiTietSanPham = await _sanPhamServices.GetAllChiTietSanPham(idSanPham);
            return Ok(lstChiTietSanPham);
        }
        #endregion

        #region LoaiSP
        [HttpGet("GetAllLoaiSPCha")]
        public async Task<IActionResult> GetAllLoaiSPCha() 
        {
            var listLsp = await _sanPhamServices.GetAllLoaiSPCha();
            return Ok(listLsp);
        }
        [HttpGet("GetAllLoaiSPCon")]
        public async Task<IActionResult> GetAllLoaiSPCon(string tenLoaiSPCha)
        {
            var listLsp = await _sanPhamServices.GetAllLoaiSPCon(tenLoaiSPCha);
            return Ok(listLsp);
        }
        #endregion

        [HttpGet("GetAllMauSac")]
        public async Task<IActionResult> GetAllMauSac()
        {
            var lstMauSac =  await _sanPhamServices.GetAllMauSac();
            return Ok(lstMauSac);
        }
        [HttpGet("GetAllKichCo")]
        public async Task<IActionResult> GetAllKichCo()
        {
            var lstKichCo = await _sanPhamServices.GetAllKichCo();
            return Ok(lstKichCo);
        }
        [HttpGet("GetAllChatLieu")]
        public async Task<IActionResult> GetAllChatLieu()
        {
            var lstChatLieu = await _sanPhamServices.GetAllChatLieu();
            return Ok(lstChatLieu);
        }
    }
}
