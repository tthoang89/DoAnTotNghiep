using AppData.Models;
using AppData.ViewModels;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace AppView.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly HttpClient httpClients;

        public KhachHangController()
        {
            httpClients = new HttpClient();
        }

        public int PageSize = 10;
        // Get ALl KH
        public async Task<IActionResult> GetAllKhachHang(int ProductPage = 1)
        {
            string apiUrl = "https://localhost:7095/api/KhachHang";
            var response = await httpClients.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var kh = JsonConvert.DeserializeObject<List<KhachHangView>>(apiData);
            return View(new PhanTrangKhachHang
            {
                listkh = kh
                       .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = kh.Count()
                }

            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLSTDByIDKH(Guid id, int ProductPage = 1)
        {
           
            string apiURL = $"https://localhost:7095/api/LichSuTichDiem/GetLSTDByIdKH?idkh={id}";
            var response = await httpClients.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<LichSuTichDiemView>>(apiData);
            return View(new PhanTrangLSTD
            {
                listLSTDs = roles
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = roles.Count()
                }

            }
                );

        }
        // tim kiem KH theo Ten Hoac SDT
        [HttpGet]
        public async Task<IActionResult> GetAllKHTheoTimKiem(string? Ten, string? SDT,int ProductPage = 1)
        {
            string apiUrl = $"https://localhost:7095/api/KhachHang/TimKiemKH?Ten={Ten}&SDT={SDT}";
            var response = await httpClients.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var kh = JsonConvert.DeserializeObject<List<KhachHangView>>(apiData);
            return View("GetAllKhachHang", new PhanTrangKhachHang
            {
                listkh = kh
                       .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = kh.Count()
                }

            });
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(KhachHangView kh, string nhaplai)
        {
            string apiUrl1 = "https://localhost:7095/api/KhachHang";
            var response1 = await httpClients.GetAsync(apiUrl1);
            string apiData1 = await response1.Content.ReadAsStringAsync();
            var kh1 = JsonConvert.DeserializeObject<List<KhachHangView>>(apiData1);
            if (kh.Password != null || kh.SDT != null || kh.Email != null)
            {
                if (kh.Password.Length < 5)
                {
                    ViewBag.MatKhau = "Mật Khẩu phải lớn hơn 5 kí tự";
                }
                if (kh.SDT.Length < 10)
                {
                    ViewBag.SDT = "Số Điện thoại không hợp lệ";
                }
                var timkiem = kh1.Where(x => x.SDT == kh.SDT).FirstOrDefault();
                if (timkiem != null)
                {
                    ViewBag.SDT = "Số Điện thoại này đã được đăng kí";
                }
                if (!kh.Email.Contains("@"))
                {
                    ViewBag.email = kh.Email.Replace("@", "%40");
                }
                var email = kh1.Where(x => x.Email == kh.Email).FirstOrDefault();
                if (email != null)
                {
                    ViewBag.email = "email đã tồn tại";

                }
                if (nhaplai != kh.Password)
                {
                    ViewBag.NhapLai = "Nhập lại mật khẩu không đúng ";
                }
                if (kh.SDT.Length >= 10 && kh.Email.Contains("@") &&   timkiem == null && email == null && nhaplai == kh.Password)
                {
                    var url = $"https://localhost:7095/api/KhachHang/PostKHView";
                    var response = await httpClients.PostAsJsonAsync(url, kh);
                    if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKhachHang");
                    return View();
                }
            }            
                return View();
        }
        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {

            string apiUrl = "https://localhost:7095/api/KhachHang/GetById?id=" + id;
            var response = await httpClients.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var NhaCungCaps = JsonConvert.DeserializeObject<KhachHangView>(apiData);
            return View(NhaCungCaps);

        }
        [HttpGet]
        public async Task<IActionResult> Updates(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/KhachHang/GetById?id={id}";
            var response = await httpClients.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var kh = JsonConvert.DeserializeObject<KhachHangView>(apiData);

            return View(kh);
        }
        [HttpPost]

        public async Task<IActionResult> Updates(KhachHangView kh)
        {
            var url =
         $"https://localhost:7095/api/KhachHang/PutKhView";
            var response = await httpClients.PutAsJsonAsync(url, kh);
            if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKhachHang");
            return View();
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var url = $"https://localhost:7095/api/KhachHang/{id}";
            var response = await httpClients.DeleteAsync(url);
            if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKhachHang");
            return BadRequest();
        }
    }
}
