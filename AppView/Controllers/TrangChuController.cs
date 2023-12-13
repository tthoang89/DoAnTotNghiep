using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;

namespace AppView.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly HttpClient _httpClient;
        public TrangChuController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/TrangChu/FilterSPHome/{index}")]
        public async Task<IActionResult> FilterSPHome(string index)
        {

            var lstsp = await _httpClient.GetFromJsonAsync<List<HomeProductViewModel>>("SanPham/getAllSPTrangChu");
            // Lấy sản phẩm mới nhất
            var loai = Convert.ToInt32(index);
            if(loai == 1)
            {
                lstsp = lstsp.OrderByDescending(c => c.NgayTao).Take(8).ToList();
            }else if(loai == 2) // Lấy sản phẩm bán chạy
            {
                lstsp = lstsp.OrderByDescending(c=> c.SLBan).Take(8).ToList();
            }
            else // Sp có điểm đánh giá cao nhất
            {
                lstsp = lstsp.OrderByDescending(c => c.SoSao).Take(8).ToList();
            }
            return Json( new { data = lstsp});
        }
    }
}
