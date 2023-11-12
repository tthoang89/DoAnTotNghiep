using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AppView.Controllers
{
    public class BanHangTaiQuayController : Controller
    {
        private readonly HttpClient _httpClient;

        public BanHangTaiQuayController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        //Giao diện bán hàng
        [HttpGet]
        public async Task<IActionResult> BanHang(Guid id)
        {
            ViewBag.IdNhanVien = id;
            var listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");
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
        
        //Lấy Hóa đơn chi tiết
        [HttpGet("/BanHangTaiQuay/getCTHD/{id}")]
        public async Task<IActionResult> getCTHD(string id)
        {
            var hdon = await _httpClient.GetFromJsonAsync<HoaDonViewModelBanHang>($"HoaDon/GetHDBanHang/{id}");
            var kh = await _httpClient.GetFromJsonAsync<List<KhachHang>>($"KhachHang");
            ViewBag.lstKH = kh;
            return PartialView("GioHang", hdon);
        }

        // Thêm hóa đơn chi tiết
        public async Task<ActionResult> addHdct(HoaDonChiTietRequest request)
        {
            try
            {
                HoaDonChiTietRequest hdct = new HoaDonChiTietRequest()
                {
                    Id = new Guid(),
                    IdChiTietSanPham = request.IdChiTietSanPham,
                    IdHoaDon = request.IdHoaDon,
                    SoLuong = request.SoLuong,
                    DonGia = request.DonGia,
                };
                var response = await _httpClient.PostAsJsonAsync("ChiTietHoaDon/saveHDCT/", hdct);
                if(response.IsSuccessStatusCode) return Json(new { success = true });
                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        //Xóa chi tiết hóa đơn
        public async Task<ActionResult> deleteHdct(HoaDonChiTietRequest request)
        {

            var response = await _httpClient.DeleteAsync($"ChiTietHoaDon/delete/{request.Id}");
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Xóa thành công" });
            return Json(new { success = false, message = "Xóa thất bại" });
        }

        //Load Modal Thanh Toan
        [HttpGet("/BanHangTaiQuay/ViewThanhToan/{id}")]
        public async Task<IActionResult> ViewThanhToan(string id)
        {
            var hd = await _httpClient.GetFromJsonAsync<HoaDon>($"HoaDon/GetById/{id}");
            var lstcthd = await _httpClient.GetFromJsonAsync<List<HoaDonChiTietViewModel>>($"ChiTietHoaDon/getByIdHD/{id}");
            var listpttt = await _httpClient.GetFromJsonAsync<List<PhuongThucThanhToan>>("HoaDon/PhuongThucThanhToan");
            //Quy đổi điểm
            var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
            var qddActive = qdd.FirstOrDefault(c => c.TrangThai == 1);
            //Kiểm tra là hóa đơn của khách có tài khoản không?
            var khachHang = "Khách lẻ";
            int? dtkh = 0;
            var response = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{id}");
            if (response == true)
            {
                var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{id}");
                var kh = await _httpClient.GetFromJsonAsync<KhachHang>($"KhachHang/GetById?id={lstd.IDKhachHang}");
                khachHang = kh.Ten;
                dtkh = kh.DiemTich;
            }
            var loginInfor = new LoginViewModel();
            string? session = HttpContext.Session.GetString("LoginInfor");
            if (session != null)
            {
                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
            }
            var nvien = await _httpClient.GetFromJsonAsync<NhanVien>($"NhanVien/GetById?id={loginInfor.Id}");
            var soluong = lstcthd.Sum(c => c.SoLuong);
            var ttien = lstcthd.Sum(c => c.SoLuong * c.DonGia);
            ViewData["lstPttt"] = listpttt;
            var hdtt = new HoaDonThanhToanViewModel()
            {
                Id = hd.ID,
                MaHD = hd.MaHD,
                NgayThanhToan = DateTime.Now,
                KhachHang = khachHang,
                TongSL = soluong,
                TongTien = ttien,
                DiemKH = dtkh,
                DiemTichHD =Convert.ToInt32(ttien* qddActive.TiLeTichDiem/qddActive.SoDiem),
                NhanVien = nvien.Ten,
                ThueVAT = (ttien * 10 / 100), // 10%
            }; 
            ViewBag.tileTieu =((double)qddActive.TiLeTieuDiem)/ ((double)qddActive.TiLeTichDiem);
            return PartialView("_ThanhToan", hdtt);
        }

        //ThanhToan
        public async Task<IActionResult> ThanhToan(HoaDonThanhToanRequest request)
        {
            var hdrequest = new HoaDonThanhToanRequest()
            {
                Id = request.Id,
                IdNhanVien = request.IdNhanVien,
                NgayThanhToan = DateTime.Now,
                IdVoucher = request.IdVoucher,
                IdPTTT = request.IdPTTT,
                TongTien = request.TongTien,
                ThueVAT = request.ThueVAT,
                DiemTichHD = request.DiemTichHD,
                DiemSD = request.DiemSD,
                TrangThai = 6,
            };
            var response = await _httpClient.PutAsJsonAsync("HoaDon/UpdateHoaDon/", hdrequest);
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Thanh toán thành công" });
            return Json(new { success = false, message = "Thanh toán thất bại" });
        }
       
        //Thêm nhanh khách hàng
        [HttpPost]
        public async Task<IActionResult> AddKhachHang(KhachHangView request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("KhachHang/PostKHView/", request);
                if (response.IsSuccessStatusCode) // Thêm khách hàng thành công -> tạo lịch sử tích điểm
                {
                    var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
                    var idqdd = qdd.FirstOrDefault(c => c.TrangThai == 1).ID;
                    var kh = await _httpClient.GetFromJsonAsync<KhachHang>($"KhachHang/getBySDT?sdt={request.SDT}");
                    var IDHD = request.IDKhachHang; // Luu tam idhd qua idkh
                                                    // ktra hd đã có lstd 
                    var checkexist = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{IDHD}");
                    if (checkexist == true) // Tồn tại-> xóa
                    {
                        var lstdexist = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{IDHD}");
                        var deletelstd = await _httpClient.DeleteAsync($"LichSuTichDiem/{lstdexist.ID}");
                    }
                    LichSuTichDiem lstd = new LichSuTichDiem()
                    {
                        Diem = 0,
                        TrangThai = 1,
                        IDKhachHang = kh.IDKhachHang,
                        IDQuyDoiDiem = idqdd,
                        IDHoaDon = IDHD,
                    };
                    string apiUrl = $"https://localhost:7095/api/LichSuTichDiem?diem=0&trangthai=1&IdKhachHang={kh.IDKhachHang}&IdQuyDoiDiem={idqdd}&IdHoaDon={IDHD}";
                    var lstdresponse = await _httpClient.PostAsync(apiUrl, null);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        //Sửa khách hàng
        public async Task<IActionResult> UpdateKHinHD(string idkh, string idhd)
        {
            try
            {
                var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
                var idqdd = qdd.FirstOrDefault(c => c.TrangThai == 1).ID;

                var checkexist = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{idhd}");
                if (checkexist == true) // Tồn tại-> sửa
                {
                    var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{idhd}");
                    string apiUrl = $"https://localhost:7095/api/LichSuTichDiem/{lstd.ID}?diem={lstd.Diem}&trangthai={lstd.TrangThai}&IdKhachHang={idkh}&IdQuyDoiDiem={lstd.IDQuyDoiDiem}&IdHoaDon={idhd}";
                    var response = await _httpClient.PutAsync(apiUrl, null);
                }
                else // Chưa có lstd-> tạo mới
                {
                    LichSuTichDiem lstd = new LichSuTichDiem()
                    {
                        Diem = 0,
                        TrangThai = 1,
                        IDKhachHang = Guid.Parse(idkh),
                        IDQuyDoiDiem = idqdd,
                        IDHoaDon = Guid.Parse(idhd),
                    };
                    string apiUrl = $"https://localhost:7095/api/LichSuTichDiem?diem=0&trangthai=1&IdKhachHang={idkh}&IdQuyDoiDiem={idqdd}&IdHoaDon={idhd}";
                    var lstdresponse = await _httpClient.PostAsync(apiUrl, null);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
            
        }
        //Check voucher

        //HÓA ĐƠN
        //Chuyển view hóa đơn
        [HttpGet("/BanHangTaiQuay/QuanLyHD")]
        public IActionResult QuanLyHD()
        {
            return PartialView("_QuanLyHoaDon");
        }
    }
}
