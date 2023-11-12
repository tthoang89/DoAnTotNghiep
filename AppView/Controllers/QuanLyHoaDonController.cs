using AppData.ViewModels.BanOffline;
using Microsoft.AspNetCore.Mvc;

namespace AppView.Controllers
{
    public class QuanLyHoaDonController : Controller
    {
        private readonly HttpClient _httpClient;

        public QuanLyHoaDonController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        //View QLHD
        public IActionResult _QuanLyHoaDon()
        {
            return View();
        }
        // Load tất cả hóa đơn
        [HttpGet]
        public async Task<IActionResult> LoadAllHoaDon(int page, int pagesize)
        {
            var listhdql = await _httpClient.GetFromJsonAsync<List<HoaDonQL>>("HoaDon/GetAllHDQly");
            listhdql = listhdql.OrderByDescending(c => c.ThoiGian).ToList();
            int totalRow = listhdql.Count;
            var model = listhdql.Skip((page - 1) * pagesize).Take(pagesize).ToArray();
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true,
            });
        }
        //Chi tiết hóa đơn 
        [HttpGet("/QuanLyHoaDonController/ViewChiTietHD/{idhd}")]
        public async Task<IActionResult> ViewChiTietHD(string idhd)
        {
            var hd = await _httpClient.GetFromJsonAsync<ChiTietHoaDonQL>($"HoaDon/ChiTietHoaDonQL/{idhd}");
            return PartialView("_ThongTinHD", hd);
        }

    }
}
