using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using SelectPdf;
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
                var bd = DateTime.Parse(filter.ngaybd);
                var kt = DateTime.Parse(filter.ngaykt);
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
                    listhdql = listhdql.Where(c => c.KhachHang.ToLower().Contains(filter.keyWord.ToLower())).ToList();
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
        ////Lọc HD 
        //public async Task<IActionResult> LocHD(FilterHD filter)
        //{
        //    var listhdql = await _httpClient.GetFromJsonAsync<List<HoaDonQL>>("HoaDon/GetAllHDQly");
        //    listhdql = listhdql.OrderByDescending(c => c.ThoiGian).ToList();
        //    //Lọc loại hd
        //    if(filter.loaiHD != null)
        //    {
        //        listhdql = listhdql.Where(c => filter.loaiHD.Contains(c.LoaiHD)).ToList();
        //    }
        //    //Lọc trạng thái
        //    if(filter.lstTT != null)
        //    {
        //        listhdql = listhdql.Where(c => filter.lstTT.Contains(c.TrangThai)).ToList();
        //    }
        //    int totalRow = listhdql.Count;
        //    var model = listhdql.Skip((filter.page - 1) * filter.pageSize).Take(filter.pageSize).ToArray();
        //    return Json(new
        //    {
        //        data = model,
        //        total = totalRow,
        //        status = true,
        //    });
        //}
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
        // Xác nhận hóa đơn đặt 
        [HttpGet("/QuanLyHoaDon/XacNhanDonDat/{idhd}")]
        public async Task<IActionResult> XacNhanDonDat(Guid idhd)
        {
            var loginInfor = new LoginViewModel();
            string? session = HttpContext.Session.GetString("LoginInfor");
            if (session != null)
            {
                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
            }
            var idnv = loginInfor.Id;

            string url = $"HoaDon?idhoadon={idhd}&trangthai={3}&idnhanvien={idnv}";
            var response = await _httpClient.PutAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
        }
        private string RenderPartialViewToString(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            var actionContext = new ActionContext(httpContext, routeData, new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = FindView(actionContext, viewName);

                if (viewResult.View == null)
                {
                    throw new InvalidOperationException($"Could not find the view '{viewName}'");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();

                return sw.ToString();
            }
        }

        private ViewEngineResult FindView(ActionContext actionContext, string viewName)
        {
            var viewEngine = _serviceProvider.GetRequiredService<ICompositeViewEngine>();
            var result = viewEngine.FindView(actionContext, viewName, false);

            if (!result.Success && result.SearchedLocations != null)
            {
                throw new InvalidOperationException($"Could not find the view '{viewName}'");
            }

            return result;
        }
        private byte[] ConvertHtmlToPdf(string html)
        {
            var converter = new HtmlToPdf();

            var doc = converter.ConvertHtmlString(html);
            using (var memoryStream = new MemoryStream())
            {
                doc.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }

        [HttpGet("/QuanLyHoaDon/ExportPDF/{id}")]
        public async Task<IActionResult> ExportPDF(string id)
        {
            var cthd = await _httpClient.GetFromJsonAsync<ChiTietHoaDonQL>($"HoaDon/ChiTietHoaDonQL/{id}");

            string html = RenderPartialViewToString("ExportHD", cthd);

            byte[] pdfBytes = ConvertHtmlToPdf(html);

            string fileName = cthd.MaHD + ".pdf";
            string filePath = Path.Combine("wwwroot", fileName);
            System.IO.File.WriteAllBytes(filePath, pdfBytes);

            string fileUrl = Url.Content("~/" + fileName);
            return Json(new { fileUrl });
        }

    }
}
