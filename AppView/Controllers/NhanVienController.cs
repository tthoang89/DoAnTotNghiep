using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AppView.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public NhanVienController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
        }

        public async Task<IActionResult> Show()
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/GetAll";
            var search = new List<NhanVien>();
            var searchName = Request.Query["searchName"].ToString();

            if (!string.IsNullOrEmpty(searchName))
            {
                search = await SearchTheoTen(searchName);
                if (search.Count == 0)
                {
                    ViewData["Message"] = "Không tìm thấy nhân viên nào với tên " + searchName;
                }
                else
                {
                    ViewBag.SearchName = searchName;
                    ViewData["searchName"] = searchName;

                    return View(search.ToList());
                }
            }
            else if (Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                ViewData["Message"] = "Vui lòng nhập tên nhân viên để tìm kiếm.";
            }

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<NhanVien>>(apiData);

            return View(users);
        }

        [HttpGet]
        public async Task<List<NhanVien>> SearchTheoTen(string name)
        {
            var apiUrl = $"https://localhost:7095/api/NhanVien/SearchTheoTen?name={name}";
            var response = await _httpClient.GetAsync(apiUrl);
            var search = JsonConvert.DeserializeObject<IEnumerable<NhanVien>>(await response.Content.ReadAsStringAsync());

            return search.ToList();
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
            if (ModelState.IsValid)
            {
                string apiUrl = $"https://localhost:7095/api/NhanVien/DangKyNhanVien?ten={nhanVien.Ten}&email={nhanVien.Email}&sdt={nhanVien.SDT}&diachi={nhanVien.DiaChi}&idVaiTro={nhanVien.IDVaiTro}&trangthai={nhanVien.TrangThai}&password={nhanVien.PassWord}";
                var reponsen = await _httpClient.PostAsync(apiUrl, null);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
            }
            return View(nhanVien);

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/{id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<NhanVien>(apiData);
            var vt = await dBContext.VaiTros.FindAsync(user.IDVaiTro);
            ViewBag.TenVaiTro = vt.Ten;
            return View(user);
        }

        public IActionResult Edit(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/{id}";
            var response = _httpClient.GetAsync(apiUrl).Result;
            var apiData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<NhanVien>(apiData);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, NhanVien nv)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = $"https://localhost:7095/api/NhanVien/{id}?ten={nv.Ten}&email={nv.Email}&password={nv.PassWord}&sdt={nv.SDT}&diachi={nv.DiaChi}&trangthai={nv.TrangThai}&idvaitro={nv.IDVaiTro}";

                var reponsen = await _httpClient.PutAsync(apiUrl, null);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
            }
            return View(nv);
            //var content = new StringContent(JsonConvert.SerializeObject(nv), Encoding.UTF8, "application/json");
            //var response = await _httpClient.PutAsync(apiUrl, content);
            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Show");
            //}
            //return View(nv);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/{id}";
            var reposen = await _httpClient.DeleteAsync(apiUrl);
            if (reposen.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return RedirectToAction("Show");
            //else
            //{
            //    // Log the error to the console.
            //    Console.WriteLine(reposen.StatusCode);
            //    Console.WriteLine(reposen.ReasonPhrase);
            //}

            //return View("DeleteError");
        }
    }
}
