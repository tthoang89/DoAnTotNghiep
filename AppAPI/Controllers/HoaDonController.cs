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
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _iHoaDonService;
        public HoaDonController()
        {
            _iHoaDonService = new HoaDonService();
        }

        // GET: api/<HoaDOnController>
        [HttpGet]
        public List<HoaDon> Get()
        {
            return _iHoaDonService.GetAllHoaDon();
        }
        [HttpPost]
        public bool CreateHoaDon(HoaDonViewModel hoaDon)
        {
            return _iHoaDonService.CreateHoaDon(hoaDon.ChiTietHoaDons, hoaDon);
        }
        [HttpPut]
        public bool UpdateTrangThai(Guid idhoadon, int trangthai)
        {
            return _iHoaDonService.UpdateTrangThaiGiaoHang(idhoadon, trangthai);
        }
    }
}
