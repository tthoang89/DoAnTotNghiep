using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace AppView.Controllers
{
    public class QuanLyKMController : Controller
    {
        private readonly HttpClient _httpClient;
        public QuanLyKMController()
        {
            _httpClient = new HttpClient();
        }
        public int PageSize = 8;
        // Get All KM
        [HttpGet]
        public async Task<IActionResult> GetAllKM(int ProductPage = 1)
        {// list khuyen mai view
            string apiURL = $"https://localhost:7095/api/KhuyenMai";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<KhuyenMaiView>>(apiData);
            ViewBag.KhuyenMaiView = roles;



            return View(new PhanTrangKhuyenMai
            {
               listkms = roles
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = roles.Count()
                }

            }) ;
        }
        // Ti Kiem ten KM
        [HttpGet]
        public async Task<IActionResult> TimKiemTenKM( string TenKM,int ProductPage = 1)
        {// list khuyen mai view
            string apiURL = $"https://localhost:7095/api/KhuyenMai";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<KhuyenMaiView>>(apiData);
            ViewBag.KhuyenMaiView = roles;



            return View("GetAllKM",new PhanTrangKhuyenMai
            {
                listkms = roles.Where(x=>x.Ten.Contains(TenKM))
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = roles.Count()
                }

            });
        }
        // Get All QL Khuyen Mai
        [HttpGet]
        public async Task<IActionResult> GetAllQLKhuyenMai(int ProductPage = 1)
        {// list khuyen mai view
            string apiURL = $"https://localhost:7095/api/KhuyenMai";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<KhuyenMaiView>>(apiData);
            ViewBag.KhuyenMaiView = roles;


        // list san pham
        https://localhost:7095/api/SanPham/getAll
            string apiURLsp = $"https://localhost:7095/api/KhuyenMai";
            var responsesp = await _httpClient.GetAsync(apiURLsp);
            var apiDatasp = await responsesp.Content.ReadAsStringAsync();
            var sanphams = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(apiDatasp);
            ViewBag.SanPhamViewModel = sanphams;
            // list bienthe

            string apiURLbt = $"https://localhost:7095/api/KhuyenMai/GetAllBienThe";
            var responsebt = await _httpClient.GetAsync(apiURLbt);
            var apiDatabt = await responsebt.Content.ReadAsStringAsync();
            var bts = JsonConvert.DeserializeObject<List<ChiTietSanPham>>(apiDatabt);
            ViewBag.BienThe = bts;
            // // list bienthe view model
            string apiURL1 = $"https://localhost:7095/api/BienThe/getAll";
            var response1 = await _httpClient.GetAsync(apiURL1);
            var apiData1 = await response1.Content.ReadAsStringAsync();
            var bienthes = JsonConvert.DeserializeObject<List<BienTheViewModel>>(apiData1);
            return View(new PhanTrangBienThe
            {
                listbienthes = bienthes
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = bienthes.Count()
                }

            });
        }
        // add km vo list SP
        public async Task<IActionResult> AddKHuyenMaiVoSP( int ProductPage = 1)
        {
            // list khuyen mai view
            string apiURL = $"https://localhost:7095/api/KhuyenMai";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<KhuyenMaiView>>(apiData);
            ViewBag.KhuyenMaiView = roles;


        // list san pham view model
        https://localhost:7095/api/SanPham/getAll
            string apiURLsp = $"https://localhost:7095/api/KhuyenMai";
            var responsesp = await _httpClient.GetAsync(apiURLsp);
            var apiDatasp = await responsesp.Content.ReadAsStringAsync();
            var sanphams = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(apiDatasp);
            ViewBag.SanPhamViewModel = sanphams;
        // list bienthe
       
            string apiURLbt = $"https://localhost:7095/api/KhuyenMai/GetAllBienThe";
            var responsebt = await _httpClient.GetAsync(apiURLbt);
            var apiDatabt = await responsebt.Content.ReadAsStringAsync();
            var bts = JsonConvert.DeserializeObject<List<ChiTietSanPham>>(apiDatabt);
            ViewBag.BienThe = bts;
            // // list bienthe View Model
            string apiURL1 = $"https://localhost:7095/api/BienThe/getAll";
            var response1 = await _httpClient.GetAsync(apiURL1);
            var apiData1 = await response1.Content.ReadAsStringAsync();
            var bienthes = JsonConvert.DeserializeObject<List<BienTheViewModel>>(apiData1);
            return View(new PhanTrangBienThe
            {
                listbienthes = bienthes
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = bienthes.Count()
                }

            });

        }
        [HttpGet]
        public async Task<IActionResult> TimKiemAllBienTheViewModel(string Ten,int ProductPage = 1)
        {
            // list khuyen mai view
            string apiURL = $"https://localhost:7095/api/KhuyenMai";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<KhuyenMaiView>>(apiData);
            ViewBag.KhuyenMaiView = roles;


        // list san pham view model
        https://localhost:7095/api/SanPham/getAll
            string apiURLsp = $"https://localhost:7095/api/KhuyenMai";
            var responsesp = await _httpClient.GetAsync(apiURLsp);
            var apiDatasp = await responsesp.Content.ReadAsStringAsync();
            var sanphams = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(apiDatasp);
            ViewBag.SanPhamViewModel = sanphams;
            // list bienthe

            string apiURLbt = $"https://localhost:7095/api/KhuyenMai/GetAllBienThe";
            var responsebt = await _httpClient.GetAsync(apiURLbt);
            var apiDatabt = await responsebt.Content.ReadAsStringAsync();
            var bts = JsonConvert.DeserializeObject<List<ChiTietSanPham>>(apiDatabt);
            ViewBag.BienThe = bts;
            // // list bienthe View Model
            string apiURL1 = $"https://localhost:7095/api/BienThe/getAll";
            var response1 = await _httpClient.GetAsync(apiURL1);
            var apiData1 = await response1.Content.ReadAsStringAsync();
            var bienthes = JsonConvert.DeserializeObject<List<BienTheViewModel>>(apiData1);
            return View("AddKHuyenMaiVoSP", new PhanTrangBienThe
            {
                listbienthes = bienthes.Where(x=>x.Ten.Contains(Ten))
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = bienthes.Count()
                }

            });

        }
        [HttpPost]
        public async Task<IActionResult> AddKHuyenMaiVoSP(Guid idKhuyenMai, List<Guid> bienthes)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/KhuyenMai/AddKmVoBT?IdKhuyenMai={idKhuyenMai}", bienthes);
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Cập nhật thành công!" });
            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<IActionResult>XoaKHuyenMaiRaSP( List<Guid> bienthes)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/KhuyenMai/XoaKmRaBT", bienthes);
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Xóa Khuyến Mãi ra  thành công!" });
            return Json(new { success = false });
        }



        // create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
       
        public async Task<IActionResult> Create(KhuyenMaiView kmv)
        {
            var response = await
           _httpClient.PostAsJsonAsync("https://localhost:7095/api/KhuyenMai", kmv);
            if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKM");
            return View();
        }
        // update
        public  IActionResult Update(Guid id)
        {
           
            var url = $"https://localhost:7095/api/KhuyenMai/{id}";
            var response = _httpClient.GetAsync(url).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            var KhuyenMais = JsonConvert.DeserializeObject<KhuyenMaiView>(result);
            return View(KhuyenMais);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KhuyenMaiView kmv)
        {
            

            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/KhuyenMai/UpdateKM", kmv);
            if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKM");
            return View();
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var url = $"https://localhost:7095/api/KhuyenMai/{id}";
            var response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKM");
            return BadRequest();
        }
    }
}
