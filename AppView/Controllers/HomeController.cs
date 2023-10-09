using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace AppView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ProductDetail()
        {
            return View();
        }
        //public IActionResult CheckOut(long tongtien)
        //{
        //    ViewData["TongTien"] = tongtien;
        //    return View();
        //}
        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult BlogDetails()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Shop()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/getAll").Result;
            List<SanPhamViewModel> lstSanpham = new List<SanPhamViewModel>();
            if(response.IsSuccessStatusCode)
            {
                lstSanpham = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
            }
            return View(lstSanpham);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Login(NhanVienViewModel nhanVien)
        //{
        //    StringContent 
        //    HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangNhap",); 
        //}
        public IActionResult Register()
        {
            return View();
        }
        //https://localhost:5001/
        //https://localhost:7095/api
    }
}