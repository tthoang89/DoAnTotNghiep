using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
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
        [HttpGet]
        public IActionResult BanHang()
        {
            ViewBag.IdNhanVien = "4fdf0898-771a-48cf-b6cf-64090a764de7";
            ViewBag.HideFooter = true;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LoadSp(int page, int pagesize)
        {
            var listsanPham = await _httpClient.GetFromJsonAsync<List<ChiTietSanPham>>("SanPham/GetAllChiTietSanPham");
            var model = listsanPham.Skip((page - 1) * pagesize).Take(pagesize);
            int totalRow = listsanPham.Count;
            return Json(new
            {
                data = listsanPham,
                total = totalRow,
                status = true,
            });
        }
        public IActionResult ThanhToanRightSide()
        {
            ViewBag.HideFooter = true;
            return View();
        }

        
        [HttpGet("/BanHangTaiQuay/getCTHD/{id}")]
        public async Task<IActionResult> getCTHD(string id)
        {
            var lstcthd = await _httpClient.GetFromJsonAsync<List<HoaDonChiTietViewModel>>($"ChiTietHoaDon/getByIdHD/{id}");
            return PartialView("GioHang",lstcthd);
        }
        public async Task<IActionResult> deleteHD(string id)
        {
            return View();
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
        public async Task<ActionResult> deleteHdct(HoaDonChiTietRequest request)
        {
            //Xóa chi tiết hóa đơn
            var response = await _httpClient.DeleteAsync($"ChiTietHoaDon/delete/{request.Id}");
            //Thêm lại số lượng cho biến thể
            var bt = await _httpClient.GetFromJsonAsync<ChiTietSanPhamViewModel>($"BienThe/getBienTheById/{request.IdBienThe}");
            //BienTheRequest btr = new BienTheRequest()
            //{
            //    ID = bt.ID,
            //    SoLuong = bt.SoLuong + request.SoLuong,
            //    GiaBan = bt.GiaBan,
            //    TrangThai =bt.TrangThai,
            //    IDKhuyenMai = bt.
            //};
            //var result = await _httpClient.PostAsJsonAsync("BienThe/saveBienThe/",btr);
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Xóa thành công" });
            return Json(new { success = false, message = "Xóa thất bại" });
        }
    }
}
