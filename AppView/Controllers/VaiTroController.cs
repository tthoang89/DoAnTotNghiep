using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace AppView.Controllers
{
    public class VaiTroController : Controller
    {
        private HttpClient _httpClient;
        public VaiTroController()
        {
                _httpClient = new HttpClient();
        }
        public IActionResult HomePageAdmin()
        {
            return View();
        }
        
        // GET: RoleController
        public async Task<IActionResult> Show()
        {
            string apiURL = $"https://localhost:7095/api/VaiTro";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<VaiTro>>(apiData);
            return View(roles);
        }

        // GET: RoleController/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro/{Id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<VaiTro>(apiData);
            return View(roles);
        }

        // GET: RoleController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VaiTro vaiTro)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro?ten={vaiTro.Ten}&Status={vaiTro.TrangThai = 1}";
            var content = new StringContent(JsonConvert.SerializeObject(vaiTro), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }
        // GET: RoleController/Edit/5
        public async Task<IActionResult> Edit(Guid Id)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro/{Id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<VaiTro>(apiData);
            return View(roles);
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, VaiTro vaiTro)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro/id={vaiTro.ID}?ten={vaiTro.Ten}&trnagthai={vaiTro.TrangThai}";
            var content = new StringContent(JsonConvert.SerializeObject(vaiTro), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }
        
    }
}
