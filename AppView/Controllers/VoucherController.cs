using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppView.Controllers
{
    public class VoucherController : Controller
    {
        private HttpClient _httpClient;
        public VoucherController()
        {
                _httpClient = new HttpClient();
        }
        public async Task<IActionResult> Show()
        {
            string apiURL = $"https://localhost:7095/api/Voucher";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<Voucher>>(apiData);
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro/{Id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<VaiTro>(apiData);
            return View(roles);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Voucher vcher)
        {
            string apiURL = $"https://localhost:7095/api/Voucher?ten={vcher.Ten}&hinhthucgiamgia={vcher.HinhThucGiamGia}&sotiencan={vcher.SoTienCan}&giatri={vcher.GiaTri}&NgayApDung={vcher.NgayApDung}&NgayKetThuc={vcher.NgayKetThuc}&soluong={vcher.SoLuong}&mota={vcher.MoTa}&trangthai={vcher.TrangThai}";
            var content = new StringContent(JsonConvert.SerializeObject(vcher), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            string apiURL = $"https://localhost:7095/api/Voucher/{Id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<Voucher>(apiData);
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, Voucher vcher)
        {
            string apiURL = $"https://localhost:7095/api/Voucher/{Id}?ten={vcher.Ten}&hinhthucgiamgia={vcher.HinhThucGiamGia}&sotiencan={vcher.SoTienCan}&giatri={vcher.GiaTri}&NgayApDung={vcher.NgayApDung}&NgayKetThuc={vcher.NgayKetThuc}&soluong={vcher.SoLuong}&mota={vcher.MoTa}&trangthai={vcher.TrangThai}";
            var content = new StringContent(JsonConvert.SerializeObject(vcher), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }
    }
}
