using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace AppView.Controllers
{
    public class KhuyenMaiController : Controller
    {
        private HttpClient _httpClient;
        public KhuyenMaiController()
        {
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> Show()
        {
            string apiURL = $"https://localhost:7095/api/KhuyenMai";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiData);
            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            string apiURL = $"https://localhost:7095/api/KhuyenMai/{Id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<KhuyenMai>(apiData);
            return View(roles);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuyenMai khmai)
        {
            string apiURL = $"https://localhost:7095/api/KhuyenMai?ten={khmai.Ten}&giatri={khmai.GiaTri}&NgayApDung={khmai.NgayApDung}&NgayKetThuc={khmai.NgayKetThuc}&mota={khmai.MoTa}&trangthai={khmai.TrangThai}";
            var content = new StringContent(JsonConvert.SerializeObject(khmai), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            string apiURL = $"https://localhost:7095/api/KhuyenMai/{Id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<KhuyenMai>(apiData);
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, KhuyenMai khmai)
        {
            string apiURL = $"https://localhost:7095/api/KhuyenMai/{Id}?ten={khmai.Ten}&giatri={khmai.GiaTri}&NgayApDung={khmai.NgayApDung}&NgayKetThuc={khmai.NgayKetThuc}&mota={khmai.MoTa}&trangthai={khmai.TrangThai}";
            var content = new StringContent(JsonConvert.SerializeObject(khmai), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }
    }
}
