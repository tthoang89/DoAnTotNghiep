using AppData.Models;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using AppView.PhanTrang;
using AppData.ViewModels;
using DocumentFormat.OpenXml.InkML;
using System.Net;

namespace AppView.Controllers
{
    public class LoaiSPController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public LoaiSPController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
        }
        public int PageSize = 8;
        // laam them 
        public async Task<IActionResult> Show(int ProductPage = 1)
        {
            string apiUrl = "https://localhost:7095/api/LoaiSP/getAll";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var LoaiSPs = JsonConvert.DeserializeObject<List<LoaiSP>>(apiData);
            var filteredLoaiSPs = LoaiSPs.Where(lsp => lsp.IDLoaiSPCha == null).ToList();
            return View(new PhanTrangLoaiSP
            {
                listlsp = filteredLoaiSPs.Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = filteredLoaiSPs.Count()
                }
            });
        }
        // Tim kiem Loai SP theo ten
        [HttpGet]
        public async Task<IActionResult> TimKiemLoaiSPTheoTen(string Ten, int ProductPage = 1)
        {
            string apiUrl = "https://localhost:7095/api/LoaiSP/getAll";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var LoaiSPs = JsonConvert.DeserializeObject<List<LoaiSP>>(apiData);
            return View(new PhanTrangLoaiSP
            {
                listlsp = LoaiSPs.Where(x => x.Ten.Contains(Ten))
                         .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = LoaiSPs.Count()
                }

            }
                 );
        }
        public async Task<IActionResult> Create()
        {
            var responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + $"https://localhost:7095/api/LoaiSP/getAll").Result;
            if (responseLoaiSP.IsSuccessStatusCode)
            {
                ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(LoaiSPRequest lsp)
        {
            lsp.ID = Guid.NewGuid();
            lsp.TrangThai = 1;
            string apiURL = $"https://localhost:7095/api/LoaiSP/save";
            if (string.IsNullOrEmpty(lsp.Ten))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập tên loại sản phẩm!";
                return View();
            }
            var content = new StringContent(JsonConvert.SerializeObject(lsp), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.ErrorMessage = "Loại sản phẩm này đã có trong danh sách";
                return View();
            }
            return View();
        }
        public async Task<IActionResult> Details(Guid id)
        {

            string apiUrl = $"https://localhost:7095/api/LoaiSP/getById/{id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var LoaiSPs = JsonConvert.DeserializeObject<LoaiSP>(apiData);
            return View(LoaiSPs);

        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/LoaiSP/getById/{id}";
            var response = _httpClient.GetAsync(apiUrl).Result;
            var apiData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<LoaiSP>(apiData);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LoaiSPRequest nv)
        {
            nv.TrangThai = 1;
            string apiUrl = $"https://localhost:7095/api/save";
            var response = await _httpClient.PutAsJsonAsync(apiUrl, nv);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetLoaiSpById(Guid id, int ProductPage = 1)
        {
            // list loai san pham con
            string apiUrl = $"https://localhost:7095/api/LoaiSP?id={id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var LoaiSPs = JsonConvert.DeserializeObject<List<LoaiSP>>(apiData);
            return View(new PhanTrangLoaiSP
            {
                listlsp = LoaiSPs.Where(x => x.TrangThai == 1)
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = LoaiSPs.Count()
                }
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateLoaiSPCon(LoaiSPRequest lsp, Guid id)
        {
            lsp.ID = Guid.NewGuid();
            lsp.TrangThai = 1;

            string apiURL = $"https://localhost:7095/api/LoaiSP/save";
            var content = new StringContent(JsonConvert.SerializeObject(lsp), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetLoaiSpById");
            }
            return View();
        }
        public async Task<IActionResult> Sua(Guid id)
        {
            var timkiem = dBContext.LoaiSPs.FirstOrDefault(x => x.ID == id);
            if (timkiem != null)
            {
                timkiem.TrangThai = timkiem.TrangThai == 0 ? 1 : 0;
                dBContext.LoaiSPs.Update(timkiem);
                dBContext.SaveChanges();
                return RedirectToAction("Show");
            }
            else
            {
                return View();
            }
        }
    }
}
