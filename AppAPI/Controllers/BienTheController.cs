using AppAPI.IServices;
using AppAPI.Services;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BienTheController : ControllerBase
    {
        private readonly IBienTheService _bienTheService;
        public BienTheController()
        {
            this._bienTheService = new BienTheService();
        }
        [HttpGet("getBienTheByIdSP/{id}")]
        public async Task<IActionResult> GetBTByIdSp(Guid id)
        {
            var listbt = await _bienTheService.GetBienTheByIdSanPham(id);
            return Ok(listbt);
        }
        [HttpGet("getBienTheById/{id}")]
        public async Task<IActionResult> GetBTById(Guid id)
        {
            var bt = await _bienTheService.GetBienTheById(id);
            return Ok(bt);
        }
        [HttpPost("saveBienThe")]
        public async Task<IActionResult> SaveBienThe(List<BienTheRequest> requests)
        {
            if (requests == null) return BadRequest();
            foreach(var bt in requests)
            {
                await _bienTheService.SaveBienThe(bt);
            }
            return Ok();
        }
        
        [HttpPost("getBTByIdValues")]
        public async Task<IActionResult> GetByValues([FromBody] BienTheTruyVan requests)
        {
            if (requests == null) return BadRequest();
            
            var bt = await _bienTheService.GetBTByListGiaTri(requests);
            return Ok(bt);
        }
        [HttpPost("setBTDefault/{idbt}")]
        public async Task<IActionResult> SetBTDefault(Guid idbt)
        {
            _bienTheService.SetBienTheDefault(idbt);
            return Ok();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBienThe(Guid id)
        {
            var bienthe = await _bienTheService.DeleteBienThe(id);
            return Ok();
        }

    }
}
