using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

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
            var lsthd = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAll");
            var lstbt = await _httpClient.GetFromJsonAsync<List<BienTheViewModel>>("BienThe/getAll");
            var lsthdcho = lsthd.Where(c => c.TrangThaiGiaoHang == 1).ToList();
            ViewData["lsthdcho"]=lsthdcho;
            ViewData["lstbthe"] = lstbt;
            return View();
        }
        public IActionResult TaoHoaDon()
        {
            HoaDon hd = new HoaDon()
            {
                NgayTao = DateTime.Now,
                IDNhanVien = Guid.Parse("C61B3646-6FF6-4E56-BA26-53E437B7C1A9"),
                TrangThaiGiaoHang = 1,
            };
            var response =  _httpClient.PostAsJsonAsync<HoaDon>("HoaDon/Offline",hd).Result;
            if (response.IsSuccessStatusCode) return RedirectToAction("BanHang");
            else return BadRequest();
        }
        [HttpGet("/BanHangTaiQuay/getCTHD/{id}")]
        public async Task<IActionResult> getCTHD(string id)
        {
            var lstcthd = await _httpClient.GetFromJsonAsync<List<HoaDonChiTietViewModel>>($"ChiTietHoaDon/getByIdHD/{id}");
            return PartialView("GioHang",lstcthd);
        }
        [HttpDelete("/BanHangTaiQuay/deleteHDCho/{id}")]
        public async Task<ActionResult> deleteHDCho(string id)
        {
            var response = await _httpClient.DeleteAsync($"HoaDon/deleteHoaDon/{id}");
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Xóa hóa đơn thành công" });
            return Json(new { success = false, message = "Xóa thất bại" });
        }
        public async Task<ActionResult> addHdct(HoaDonChiTietRequest request)
        {
            HoaDonChiTietRequest hdct = new HoaDonChiTietRequest()
            {
                Id = new Guid(),
                IdBienThe = request.IdBienThe,
                IdHoaDon = request.IdHoaDon,
                SoLuong = request.SoLuong,
                DonGia= request.DonGia,
            };
            var response = await _httpClient.PostAsJsonAsync("ChiTietHoaDon/saveHDCT/",hdct);
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Xóa hóa đơn thành công" });
            return Json(new { success = false, message = "Xóa thất bại" });
        }
        [HttpDelete("/BanHangTaiQuay/deleteHdct/{id}")]
        public async Task<ActionResult> deleteHdct(string id)
        {
            var response = await _httpClient.DeleteAsync($"ChiTietHoaDon/delete/{id}");
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Xóa thành công" });
            return Json(new { success = false, message = "Xóa thất bại" });
        }
    }
}
