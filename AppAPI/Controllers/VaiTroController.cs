using AppAPI.IServices;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly IVaiTroService _vaiTroService;

        public VaiTroController(IVaiTroService vaiTroService)
        {
            _vaiTroService = vaiTroService;
        }

        // GET: api/<VaiTroController>
        [HttpGet]
        public async  Task<IActionResult> GetAllVaiTros()
        {
            var vaitro = await _vaiTroService.GetAllVaiTro();
            return Ok(vaitro);
        }

        // GET api/<VaiTroController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vaitro = await _vaiTroService.GetVaiTroById(id);
            if(vaitro == null)
            {
                return NotFound();
            }
            return Ok(vaitro);
        }

        // POST api/<VaiTroController>
        [HttpPost]
        public async Task<IActionResult> AddVaiTro(VaiTro vaiTro)
        {
            if(vaiTro == null)
            {
                return BadRequest();
            }
            else
            {
                var vaitro = await _vaiTroService.CreateVaiTro(vaiTro);
                return Ok(vaitro);
            }
        }


        // PUT api/<VaiTroController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVaiTro(Guid id, VaiTro vaiTro)
        {
            var vt = await _vaiTroService.UpdateVaiTro(id, vaiTro);
            if(vt == null)
            {
                return NotFound();
            }
            return Ok(vt);
            
        }

        // DELETE api/<VaiTroController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaiTro(Guid id)
        {
            var vt = await _vaiTroService.DeleteVaiTro(id);
            return Ok();
        }
    }
}
