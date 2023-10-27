using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly IChiTietHoaDonService _idchiTietHoaDon;
        public ChiTietHoaDonController()
        {
            _idchiTietHoaDon = new ChiTietHoaDonService();
        }
        [HttpGet]
        public List<ChiTietHoaDon> getAll()
        {
            return _idchiTietHoaDon.GetAllCTHoaDon();
        }
        [HttpPost]
        public bool Create(ChiTietHoaDon chiTietHoaDon)
        {
            return _idchiTietHoaDon.CreateCTHoaDon(chiTietHoaDon);
        }
        [HttpPut]
        public bool Update(ChiTietHoaDon chiTietHoaDon)
        {
            return _idchiTietHoaDon.UpdateCTHoaDon(chiTietHoaDon);
        }
        [HttpDelete]
        public bool Delete(Guid id)
        {
            return _idchiTietHoaDon.DeleteCTHoaDon(id);
        }
    }
}
