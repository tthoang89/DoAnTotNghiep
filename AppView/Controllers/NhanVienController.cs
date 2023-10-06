using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppView.Controllers
{
    public class NhanVienController : Controller
    {
        private HttpClient _httpClient;
        public NhanVienController()
        {
            _httpClient = new HttpClient();
        }
        // GET: UserController
        public async Task<IActionResult> Show()
        {
            string apiUrl = "https://localhost:7095/api/NhanVien/GetAll";
            var response = await _httpClient.GetAsync(apiUrl);
            string apidata = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<NhanVien>>(apidata);
            return View(users);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nhanVien)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/DangKyNhanVien?ten={nhanVien.Ten}&email={nhanVien.Email}&sdt={nhanVien.SDT}&diachi={nhanVien.DiaChi}&idVaiTro={nhanVien.IDVaiTro}&trangthai={nhanVien.TrangThai}&password={nhanVien.PassWord}";
            var content = new StringContent(JsonConvert.SerializeObject(nhanVien), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }

        // GET: UserController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/{id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apidata = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<NhanVien>(apidata);
            return View(user);
        }


        // GET: UserController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/{id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apidata = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<NhanVien>(apidata);
            return View(user);
        }
        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, NhanVien nv)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/{nv.ID}?ten={nv.Ten}&email={nv.Email}&sdt={nv.SDT}&diachi={nv.DiaChi}&idVaiTro={nv.IDVaiTro}&trangthai={nv.TrangThai}&password={nv.PassWord}";
            var content = new StringContent(JsonConvert.SerializeObject(nv), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View(nv);
        }
        [HttpGet]
        public async Task<IActionResult> Edit2(Guid id)
        {
            string apiUrl = $"https://localhost:7109/api/User/{id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apidata = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<NhanVien>(apidata);
            return View(user);
        }
    }
}
