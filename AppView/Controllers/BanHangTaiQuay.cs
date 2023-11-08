using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        //Giao diện bán hàng
        [HttpGet]
        public async Task<IActionResult> BanHang()
        {
            ViewBag.IdNhanVien = "d34a62d4-a0c1-4ea2-8a1c-4212c952abe9";
            var listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");
            listhdcho = listhdcho.OrderByDescending(c => c.NgayTao).ToList();
            ViewData["lsthdcho"] = listhdcho;
            var listpttt = await _httpClient.GetFromJsonAsync<List<PhuongThucThanhToan>>("HoaDon/PhuongThucThanhToan");
            ViewData["lstPttt"] = listpttt;
            return View();
        }
        // Load Sản phẩm
        [HttpGet]
        public async Task<IActionResult> LoadSp(int page, int pagesize)
        {
            var listsanPham = await _httpClient.GetFromJsonAsync<List<ChiTietSanPhamViewModel>>("SanPham/GetAllChiTietSanPham");
            var model = listsanPham.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            int totalRow = listsanPham.Count;
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true,
            });
        }
        //Load hóa đơn chờ
        [HttpGet]
        public async Task<IActionResult> LoadHoaDonCho()
        {
            var listhd = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAll");
            var listhdcho = listhd.Where(c => c.TrangThaiGiaoHang == 1).ToList().OrderByDescending(c => c.NgayTao);
            return Json(new { data = listhdcho });
        }
        
        //Load Modal Thanh Toan
        [HttpGet("/BanHangTaiQuay/ViewThanhToan/{id}")]
        public async Task<IActionResult> ViewThanhToan(string id)
        {
            var hd = await _httpClient.GetFromJsonAsync<HoaDon>($"HoaDon/GetById/{id}");
            var lstcthd = await _httpClient.GetFromJsonAsync<List<HoaDonChiTietViewModel>>($"ChiTietHoaDon/getByIdHD/{id}");
            var listpttt = await _httpClient.GetFromJsonAsync<List<PhuongThucThanhToan>>("HoaDon/PhuongThucThanhToan");
            //Kiểm tra là hóa đơn của khách có tài khoản không?
            var khachHang = "Khách lẻ";
            var response = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{id}");
            if (response == true)
            {
                var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{id}");
                var kh = await _httpClient.GetFromJsonAsync<KhachHang>($"KhachHang/{id}");
                khachHang = kh.Ten;
            }
            var nvien = await _httpClient.GetFromJsonAsync<NhanVien>($"NhanVien/{hd.IDNhanVien}");
            var soluong = lstcthd.Sum(c => c.SoLuong);
            var ttien = lstcthd.Sum(c => c.SoLuong * c.DonGia);
            ViewData["lstPttt"] = listpttt;
            var hdtt = new HoaDonThanhToanViewModel()
            {
                Id = hd.ID,
                NgayThanhToan = DateTime.Now,
                KhachHang = khachHang,
                TongSL = soluong,
                TongTien = ttien,
                NhanVien = nvien.Ten,
            };
            return PartialView("_ThanhToan", hdtt);
        }
        //Lấy Hóa đơn chi tiết
        [HttpGet("/BanHangTaiQuay/getCTHD/{id}")]
        public async Task<IActionResult> getCTHD(string id)
        {
            var lstcthd = await _httpClient.GetFromJsonAsync<List<HoaDonChiTietViewModel>>($"ChiTietHoaDon/getByIdHD/{id}");
            lstcthd.Reverse();
            return PartialView("GioHang", lstcthd);
        }
        
        // Thêm hóa đơn chi tiết
        public async Task<ActionResult> addHdct(HoaDonChiTietRequest request)
        {
            HoaDonChiTietRequest hdct = new HoaDonChiTietRequest()
            {
                Id = new Guid(),
                IdChiTietSanPham = request.IdChiTietSanPham,
                IdHoaDon = request.IdHoaDon,
                SoLuong = request.SoLuong,
                DonGia= request.DonGia,
            };
            var response = await _httpClient.PostAsJsonAsync("ChiTietHoaDon/saveHDCT/",hdct);
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Thêm thành công" });
            else return Json(new { success = false, message = "Lỗi thêm sản phẩm" });
        }
        //Xóa chi tiết hóa đơn
        public async Task<ActionResult> deleteHdct(HoaDonChiTietRequest request)
        {
            
            var response = await _httpClient.DeleteAsync($"ChiTietHoaDon/delete/{request.Id}");
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Xóa thành công" });
            return Json(new { success = false, message = "Xóa thất bại" });
        }
        //ThanhToan
        [HttpPost]
        public async Task<IActionResult> ThanhToan(HoaDonThanhToanRequest request)
        {
            var hdrequest = new HoaDonThanhToanRequest()
            {
                Id = request.Id,
                IdNhanVien = Guid.Parse("d34a62d4-a0c1-4ea2-8a1c-4212c952abe9"),
                NgayThanhToan = DateTime.Now,
                IdVoucher = request.IdVoucher,
                TrangThai = 7,
            };
            var response = await _httpClient.PutAsJsonAsync("HoaDon/UpdateHoaDon/",hdrequest);
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Thanh toán thành công" });
            return Json(new { success = true, message = "Thanh toán thất bại" });
        }
        //HÓA ĐƠN
        // Load tất cả hóa đơn
        [HttpGet]
        public async Task<IActionResult> LoadAllHoaDon(int page, int pagesize)
        {
            var listhd = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAll");
            listhd = listhd.Where(c=>c.TrangThaiGiaoHang != 1).OrderByDescending(c=>c.NgayTao).ToList();
            int totalRow = listhd.Count;
            var model = listhd.Skip((page - 1) * pagesize).Take(pagesize).ToArray();
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true,
            });
        }
        [HttpGet("/BanHangTaiQuay/QuanLyHD")]
        public IActionResult QuanLyHD()
        {
            return PartialView("_QuanLyHoaDon");
        }
    }
}
