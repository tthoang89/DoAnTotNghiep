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
            lsp.TrangThai = 1;
            string apiURL = $"https://localhost:7095/api/LoaiSP/save";
            var content = new StringContent(JsonConvert.SerializeObject(lsp), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
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
            //var url = $"https://localhost:7095/api/LoaiSP/getById/{id}";
            var responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + $"https://localhost:7095/api/LoaiSP/getAll").Result;
            if (responseLoaiSP.IsSuccessStatusCode)
            {
                ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LoaiSPRequest lsp, Guid id)
        {
            if (lsp == null) return BadRequest();
            string apiURL = $"https://localhost:7095/api/LoaiSP/save";
            var content = new StringContent(JsonConvert.SerializeObject(lsp), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            ModelState.AddModelError("", "Có lỗi xảy ra khi chỉnh sửa.");
            return View(lsp);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoaiSpById(Guid id, int ProductPage = 1)
        {
            // list khuyen mai view
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
