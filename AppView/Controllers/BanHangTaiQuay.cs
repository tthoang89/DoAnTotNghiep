using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.Controllers
{
    public class BanHangTaiQuay : Controller
    {
        private readonly HttpClient _httpClient;
        public BanHangTaiQuay()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        public async Task<IActionResult> BanHang()
        {
            var lsthd = await _httpClient.GetFromJsonAsync<List<HoaDon>>(_httpClient.BaseAddress+"HoaDon/GetAll");
            var lstbt = await _httpClient.GetFromJsonAsync<List<BienTheViewModel>>(_httpClient.BaseAddress + "BienThe/getAll");
            var lsthdcho = lsthd.Where(c => c.TrangThaiGiaoHang == 2).ToList();
            ViewData["lsthdcho"]=lsthdcho;
            ViewData["lstbthe"] = lstbt;
            return View();
        }
        public async Task<IActionResult> GioHangTaiQuay()
        {
            var lstbt = await _httpClient.GetFromJsonAsync<List<BienTheViewModel>>(_httpClient.BaseAddress + "BienThe/getAll");
            return PartialView("GioHang", lstbt);
        }
        
        public async Task<IActionResult> TaoHoaDon()
        {
            HoaDon hd = new HoaDon()
            {
                NgayTao = DateTime.Now,
                IDNhanVien = Guid.Parse("C61B3646-6FF6-4E56-BA26-53E437B7C1A9"),
                TrangThaiGiaoHang = 2,
            };
            var response =  _httpClient.PostAsJsonAsync<HoaDon>("HoaDon",hd).Result;
            if (response.IsSuccessStatusCode) return RedirectToAction("BanHang");
            else return BadRequest();
        }
    }
}
