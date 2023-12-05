using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System.Globalization;
using System.Net;

namespace AppView.Controllers
{

    public class QuanLyHoaDonController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;
        public QuanLyHoaDonController(IServiceProvider serviceProvider, ITempDataProvider tempDataProvider)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }
        //View QLHD
        public IActionResult _QuanLyHoaDon()
        {
            return View();
        }
        // Load tất cả hóa đơn
        public async Task<IActionResult> LoadAllHoaDon(FilterHD filter)
        {
            var listhdql = await _httpClient.GetFromJsonAsync<List<HoaDonQL>>("HoaDon/GetAllHDQly");
            listhdql = listhdql.OrderByDescending(c => c.ThoiGian).ToList();
            //Lọc thời gian
            if (filter.ngaybd != null)
            {
                string[] formats = { "MM/dd/yyyy HH:mm:ss" };
                DateTime parsedDate = DateTime.ParseExact(filter.ngaybd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime parsedDate1 = DateTime.ParseExact(filter.ngaykt, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                string output = parsedDate.ToString("MM/dd/yyyy HH:mm:ss");
                string output1 = parsedDate1.ToString("MM/dd/yyyy HH:mm:ss");
                var bd = DateTime.ParseExact(output, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                var kt = DateTime.ParseExact(output1, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                listhdql = listhdql.Where(c => c.ThoiGian >= bd && c.ThoiGian <= kt).ToList();
            }
            //Tìm kiếm 
            if (filter.keyWord != null)
            {
                if (filter.loaitk == 1)
                {
                    listhdql = listhdql.Where(c => c.MaHD.ToLower().Contains(filter.keyWord.ToLower())).ToList();
                }
                else
                {
                    listhdql = listhdql.Where(c => c.KhachHang.ToLower().Contains(filter.keyWord.ToLower()) || c.SDT.Contains(filter.keyWord)).ToList();
                }
            }
            //Lọc kênh
            if (filter.loaiHD != null)
            {
                listhdql = listhdql.Where(c => filter.loaiHD.Contains(c.LoaiHD)).ToList();
            }
            //Lọc trạng thái
            if (filter.lstTT != null)
            {
                listhdql = listhdql.Where(c => filter.lstTT.Contains(c.TrangThai)).ToList();
            }

            // Tổng tiền hàng
            var tth = listhdql.Sum(c => c.TongTienHang);
            // Tổng tiền khách đã trả
            var tktra = listhdql.Sum(c => c.KhachDaTra);

            int totalRow = listhdql.Count;
            var model = listhdql.Skip((filter.page - 1) * filter.pageSize).Take(filter.pageSize).ToArray();
            //Lọc loại hd
            return Json(new
            {
                tienhang = tth,
                khachtra = tktra,
                data = model,
                total = totalRow,
                status = true,
            });
        }

        //Chi tiết hóa đơn 
        [HttpGet("/QuanLyHoaDon/ViewChiTietHD/{idhd}")]
        public async Task<IActionResult> ViewChiTietHD(string idhd)
        {
            var hd = await _httpClient.GetFromJsonAsync<ChiTietHoaDonQL>($"HoaDon/ChiTietHoaDonQL/{idhd}");
            return PartialView("_ThongTinHD", hd);
        }
        
        //Hủy hóa đơn
        [HttpGet("/QuanLyHoaDon/HuyHD")]
        public async Task<IActionResult> HuyHD(Guid idhd, string ghichu)
        {
            var loginInfor = new LoginViewModel();
            string? session = HttpContext.Session.GetString("LoginInfor");
            if (session != null)
            {
                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
            }
            var idnv = loginInfor.Id;
            string url = $"HoaDon/HuyHD?idhd={idhd}&idnv={idnv}&Ghichu={ghichu}";
            var response = await _httpClient.PutAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
        }
        // Cập nhật trạng thái
        public async Task<IActionResult> DoiTrangThai(Guid idhd,int trangthai)
        {
            var loginInfor = new LoginViewModel();
            string? session = HttpContext.Session.GetString("LoginInfor");
            if (session != null)
            {
                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
            }
            var idnv = loginInfor.Id;

            string url = $"HoaDon?idhoadon={idhd}&trangthai={trangthai}&idnhanvien={idnv}";
            var response = await _httpClient.PutAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
        }
        
        [HttpGet("/Admin/QuanLyHoaDon/ExportPDF/{idhd}")]
        public async Task<IActionResult> ExportPDF(Guid idhd)
        {
            var cthd = await _httpClient.GetFromJsonAsync<ChiTietHoaDonQL>($"HoaDon/ChiTietHoaDonQL/{idhd}");
            var view = new ViewAsPdf("ExportHD", cthd)
            {
                FileName = $"{cthd.MaHD}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
            return view;
        }

    }
}
