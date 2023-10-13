using AppData.Models;
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
        private readonly string KeyCart = "GioHang";

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
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ProductDetail()
        {
            return View();
        }
        
        public IActionResult Shop()
        {
            HttpResponseMessage responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + "LoaiSP/getAll").Result;
            if (responseLoaiSP.IsSuccessStatusCode)
            {
                ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
            }
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/getAll").Result;
            List<SanPhamViewModel> lstSanpham = new List<SanPhamViewModel>();
            if(response.IsSuccessStatusCode)
            {
                lstSanpham = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
            }
            return View(lstSanpham);
        }
        #region Cart
        [HttpGet]
        public IActionResult ShoppingCart()
        {
            List<BienTheViewModel> bienThes = new List<BienTheViewModel>();
            if (HttpContext.Session.GetString(KeyCart) != null)
            {
				bienThes = JsonConvert.DeserializeObject<List<BienTheViewModel>>(HttpContext.Session.GetString(KeyCart));
			}      
            long tongtien = 0;
            foreach(var x in bienThes)
            {
                tongtien += x.GiaBan;
            }
            TempData["TongTien"] = tongtien.ToString("n0");
            TempData["ListBienThe"] = bienThes;
            return View(bienThes);
        }
        public void AddToCart(Guid id)
        { 
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"BienThe/getBienTheById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                List<BienTheViewModel> bienThes;
                BienTheViewModel bienThe = JsonConvert.DeserializeObject<BienTheViewModel>(response.Content.ReadAsStringAsync().Result);
                string? result = HttpContext.Session.GetString(KeyCart);
                if (string.IsNullOrEmpty(result))
                {
                    bienThe.SoLuong = 1;
                    bienThes = new List<BienTheViewModel>() { bienThe };
                }
                else
                {
                    bienThes = JsonConvert.DeserializeObject<List<BienTheViewModel>>(result);
                    if (bienThes.Contains(bienThe))
                    {
                        //Sua 
                        bienThe.SoLuong++;
                    }
                    else
                    {
                        bienThe.SoLuong = 1;
                        bienThes.Add(bienThe);
                    }                    
                }
                HttpContext.Session.SetString(KeyCart, JsonConvert.SerializeObject(bienThes));
            }
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(NhanVienViewModel nhanVien)
        {
            //https://localhost:7095/api/QuanLyNguoiDung/DangNhap?email=tam%40gmail.com&password=chungtam2003
            string email = nhanVien.Email.Replace("@","%40");
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangNhap?email={email}&password={nhanVien.Password}").Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("UserName",JsonConvert.DeserializeObject<NhanVien>(response.Content.ReadAsStringAsync().Result).Ten);                
            }
            return RedirectToAction("Index");
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        #endregion

        #region CheckOut
        [HttpGet]
        public IActionResult CheckOut()
        {
			return View();
        }
        [HttpPost]
        public IActionResult Pay(HoaDonViewModel hoaDon)
        {
            List<ChiTietHoaDonViewModel> lstChiTietHoaDon = new List<ChiTietHoaDonViewModel>();
            foreach(var item in TempData["ListBienThe"] as List<BienTheViewModel>)
            {
                ChiTietHoaDonViewModel chiTietHoaDon = new ChiTietHoaDonViewModel();
                chiTietHoaDon.IDBienThe = item.ID;
                chiTietHoaDon.SoLuong = item.SoLuong;
                chiTietHoaDon.DonGia = item.GiaBan;
                lstChiTietHoaDon.Add(chiTietHoaDon);
            }
            hoaDon.ChiTietHoaDons = lstChiTietHoaDon;
            hoaDon.PhuongThucThanhToan = "Mac dinh";
            hoaDon.DiaChi = "Mac dinh";
            hoaDon.TienShip = 0;
            HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon/CreateHoaDon", hoaDon).Result;
            if (response.IsSuccessStatusCode) return RedirectToAction("Index");
            else return BadRequest();
        }
        //public IActionResult CheckOut(long tongtien)
        //{
        //    ViewData["TongTien"] = tongtien;
        //    return View();
        //}
        #endregion

        #region Other
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
        //https://localhost:5001/
        //https://localhost:7095/api
        #endregion
    }
}