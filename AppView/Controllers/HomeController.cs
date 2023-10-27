using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using X.PagedList;

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

        public IActionResult Shop(int? pages)
        {
            HttpResponseMessage responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + "LoaiSP/getAll").Result;
            if (responseLoaiSP.IsSuccessStatusCode)
            {
                ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
            }
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/getAll").Result;
            List<SanPhamViewModel> lstSanpham = new List<SanPhamViewModel>();
            if (response.IsSuccessStatusCode)
            {
                lstSanpham = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
            }
            int pageSize = 20;
            int pageNumber = pages == null || pages < 0 ? 1 : pages.Value;
            PagedList<SanPhamViewModel> lst = new PagedList<SanPhamViewModel>(lstSanpham, pageNumber, pageSize);
            return View(lst);
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
            foreach (var x in bienThes)
            {
                tongtien += x.GiaBan * x.SoLuong;
            }
            TempData["TongTien"] = tongtien.ToString("n0");
            TempData["ListBienThe"] = JsonConvert.SerializeObject(bienThes);
            return View(bienThes);
        }
        [HttpPost]
        public ActionResult AddToCart(string id)
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
                    var tempBienThe = bienThes.FirstOrDefault(x => x.ID == bienThe.ID);
                    if (tempBienThe != null)
                    {
                        //Sua 
                        tempBienThe.SoLuong++;
                    }
                    else
                    {
                        bienThe.SoLuong = 1;
                        bienThes.Add(bienThe);
                    }
                }
                HttpContext.Session.SetString(KeyCart, JsonConvert.SerializeObject(bienThes));
                return Json(new { success = true, message = "Add to cart successfully" });
            }
            else return Json(new { success = false, message = "Add to cart fail" });
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            //https://localhost:7095/api/QuanLyNguoiDung/DangNhap?email=tam%40gmail.com&password=chungtam200396
            if (login.Contains('@'))
            {
                login = login.Replace("@", "%40");
            }
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangNhap?lg={login}&password={password}").Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("UserName", JsonConvert.DeserializeObject<LoginViewModel>(response.Content.ReadAsStringAsync().Result).Ten);
                HttpContext.Session.SetString("Role", JsonConvert.DeserializeObject<LoginViewModel>(response.Content.ReadAsStringAsync().Result).vaiTro.ToString());
                //HttpContext.Session.SetString("UserID", JsonConvert.DeserializeObject<LoginViewModel>(response.Content.ReadAsStringAsync().Result).ID);
                return RedirectToAction("Index");
            }
            else return BadRequest();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(KhachHangViewModel khachHang)
        {
            khachHang.Id = Guid.NewGuid();
            HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "KhachHang", khachHang).Result;
            if (response.IsSuccessStatusCode) return RedirectToAction("Login");
            return BadRequest();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        #endregion

        #region CheckOut
        [HttpGet]
        public IActionResult CheckOut()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Pay(HoaDonViewModel hoaDon)
        {
            List<ChiTietHoaDonViewModel> lstChiTietHoaDon = new List<ChiTietHoaDonViewModel>();
            string temp = TempData["ListBienThe"] as string;
            foreach (var item in JsonConvert.DeserializeObject<List<BienTheViewModel>>(temp))
            {
                ChiTietHoaDonViewModel chiTietHoaDon = new ChiTietHoaDonViewModel();
                chiTietHoaDon.IDBienThe = item.ID;
                chiTietHoaDon.SoLuong = item.SoLuong;
                chiTietHoaDon.DonGia = item.GiaBan;
                lstChiTietHoaDon.Add(chiTietHoaDon);
            }
            hoaDon.ChiTietHoaDons = lstChiTietHoaDon;
            hoaDon.PhuongThucThanhToan = "Mac dinh";
            hoaDon.Diem = 0;
            string tongTien = TempData["TongTien"] as string;
            hoaDon.TongTien = Convert.ToInt32(tongTien.Replace(",", ""));
            HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
            if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Pay successfully" });
            else return Json(new { success = false, message = "Pay fail" }); ;
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