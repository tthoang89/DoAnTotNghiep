using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Web.Helpers;
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
        #region SanPham
        [HttpGet]
        public IActionResult Shop(int? pages)
        {
            HttpResponseMessage responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + "LoaiSP/getAll").Result;
            if (responseLoaiSP.IsSuccessStatusCode)
            {
                ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
            }
            HttpResponseMessage responseMauSac = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllMauSac").Result;
            if (responseMauSac.IsSuccessStatusCode)
            {
                ViewData["listMauSac"] = JsonConvert.DeserializeObject<List<MauSac>>(responseMauSac.Content.ReadAsStringAsync().Result);
            }
            HttpResponseMessage responseKichCo = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllKichCo").Result;
            if (responseKichCo.IsSuccessStatusCode)
            {
                ViewData["listKichCo"] = JsonConvert.DeserializeObject<List<KichCo>>(responseKichCo.Content.ReadAsStringAsync().Result);
            }
            HttpResponseMessage responseChatLieu = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChatLieu").Result;
            if (responseChatLieu.IsSuccessStatusCode)
            {
                ViewData["listChatLieu"] = JsonConvert.DeserializeObject<List<ChatLieu>>(responseChatLieu.Content.ReadAsStringAsync().Result);
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
        [HttpGet]
        public async Task<IActionResult> ProductDetail(string idSanPham)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChiTietSanPhamHome?idSanPham="+ idSanPham);
            var chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamViewModelHome>(response.Content.ReadAsStringAsync().Result);
            return View(chiTietSanPham);
        }
        #endregion
        #region Filter
        public IActionResult GetFilteredProducts([FromBody] FilterData filter)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/getAll").Result;
            List<SanPhamViewModel> lstSanpham = new List<SanPhamViewModel>();
            if (response.IsSuccessStatusCode)
            {
                lstSanpham = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
            }
            List<SanPhamViewModel> lstSanphamfn = new List<SanPhamViewModel>();
            //price-filter
            if (filter.priceRange != null && filter.priceRange.Count > 0 && !filter.priceRange.Contains("all"))
            {
                foreach (var item in filter.priceRange)
                {
                    if (item == "1")
                    {
                        foreach (var x in lstSanpham.Where(p => p.GiaBan < 100000).ToList())
                        {
                            lstSanphamfn.Add(x);
                        }
                    }else if (item == "2")
                    {
                        foreach (var x in lstSanpham.Where(p => p.GiaBan >= 100000 && p.GiaBan < 200000).ToList())
                        {
                            lstSanphamfn.Add(x);
                        }
                    }
                    else if (item == "3")
                    {
                        foreach (var x in lstSanpham.Where(p => p.GiaBan >= 200000 && p.GiaBan < 300000).ToList())
                        {
                            lstSanphamfn.Add(x);
                        }
                    }
                    else if (item == "4")
                    {
                        foreach (var x in lstSanpham.Where(p => p.GiaBan >= 300000 && p.GiaBan < 400000).ToList())
                        {
                            lstSanphamfn.Add(x);
                        }
                    }
                    else if (item == "5")
                    {
                        foreach (var x in lstSanpham.Where(p => p.GiaBan >= 400000 && p.GiaBan < 500000).ToList())
                        {
                            lstSanphamfn.Add(x);
                        }
                    }
                    else if (item == "6")
                    {
                        foreach (var x in lstSanpham.Where(p => p.GiaBan > 500000).ToList())
                        {
                            lstSanphamfn.Add(x);
                        }
                    }
                }
            }
            else
            {
                lstSanphamfn = lstSanpham;
            }
            //loaiSP-filter
            List<SanPhamViewModel> lsttam = new List<SanPhamViewModel>();
            List<SanPhamViewModel> lsttam1 = new List<SanPhamViewModel>();
            if (filter.loaiSP != null && filter.loaiSP.Count > 0)
            {
                foreach (var x in filter.loaiSP)
                {
                    lsttam = lstSanphamfn.Where(p => p.LoaiSP == x).ToList();
                    foreach (var item in lsttam)
                    {
                        if (lsttam1.FirstOrDefault(p=>p.ID == item.ID) == null)
                        {
                            lsttam1.Add(item);
                        }
                    }
                }
                lstSanphamfn = lsttam1;
            }
            //Search
            if (filter.search != null)
            {
                lstSanphamfn = lstSanphamfn.Where(p=>p.Ten.ToLower().Contains(filter.search.ToLower())).ToList();
            }

            //color-filter
            List<SanPhamViewModel> lstmautam = new List<SanPhamViewModel>();
            List<SanPhamViewModel> lstmautam1 = new List<SanPhamViewModel>();
            if (filter.mauSac != null && filter.mauSac.Count > 0)
            {
                foreach (var x in filter.mauSac)
                {
                    lstmautam = lstSanphamfn.Where(p => p.IDMauSac == x).ToList();
                    foreach (var item in lstmautam)
                    {
                        if (lstmautam1.FirstOrDefault(p => p.ID == item.ID) == null)
                        {
                            lstmautam1.Add(item);
                        }
                    }
                }
                lstSanphamfn = lstmautam1;
            }
            //size-filter
            List<SanPhamViewModel> lstcotam = new List<SanPhamViewModel>();
            List<SanPhamViewModel> lstcotam1 = new List<SanPhamViewModel>();
            if (filter.kichCo != null && filter.kichCo.Count > 0)
            {
                foreach (var x in filter.kichCo)
                {
                    lstcotam = lstSanphamfn.Where(p => p.IDKichCo == x).ToList();
                    foreach (var item in lstcotam)
                    {
                        if (lstcotam1.FirstOrDefault(p => p.ID == item.ID) == null)
                        {
                            lstcotam1.Add(item);
                        }
                    }
                }
                lstSanphamfn = lstcotam1;
            }
            //material-filter
            List<SanPhamViewModel> lstchatlieutam = new List<SanPhamViewModel>();
            List<SanPhamViewModel> lstchatlieutam1 = new List<SanPhamViewModel>();
            if (filter.chatLieu != null && filter.chatLieu.Count > 0)
            {
                foreach (var x in filter.chatLieu)
                {
                    lstchatlieutam = lstSanphamfn.Where(p => p.IDChatLieu == x).ToList();
                    foreach (var item in lstchatlieutam)
                    {
                        if (lstchatlieutam1.FirstOrDefault(p => p.ID == item.ID) == null)
                        {
                            lstchatlieutam1.Add(item);
                        }
                    }
                }
                lstSanphamfn = lstchatlieutam1;
            }
            //sort
            if (filter.sortSP != null)
            {
                if (filter.sortSP == "2")
                {
                    lstSanphamfn = lstSanphamfn.OrderBy(p => p.GiaBan).ToList();
                }
                else if (filter.sortSP == "3")
                {
                    lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.GiaBan).ToList();
                }
                else if (filter.sortSP == "4")
                {
                    lstSanphamfn = lstSanphamfn.OrderBy(p => p.GiaBan).ToList();
                }
                else if (filter.sortSP == "5")
                {
                    lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.GiaBan).ToList();
                }
                else if (filter.sortSP == "6")
                {
                    lstSanphamfn = lstSanphamfn.OrderBy(p => p.NgayTao).ToList();
                }
                else if (filter.sortSP == "7")
                {
                    lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.NgayTao).ToList();
                }
            }
            List<SanPhamViewModel>lstSanPhamfnR = new List<SanPhamViewModel>();
            foreach (var item in lstSanphamfn)
            {
                if (item.TrangThaiCTSP == 1)
                {
                    lstSanPhamfnR.Add(item);
                }
                else
                {
                    SanPhamViewModel sanPhamViewModel = lstSanpham.FirstOrDefault(p=>p.ID == item.ID && p.TrangThaiCTSP == 1);
                    if (lstSanPhamfnR.FirstOrDefault(p=>p.ID == sanPhamViewModel.ID) == null)
                    {
                        lstSanPhamfnR.Add(sanPhamViewModel);
                    }
                }
            }

            return PartialView("_ReturnProducts", lstSanPhamfnR);
        }
        #endregion

        #region Cart
        [HttpGet]
        public IActionResult ShoppingCart()
        {
            List<ChiTietSanPhamViewModelAdmin> bienThes = new List<ChiTietSanPhamViewModelAdmin>();
            if (HttpContext.Session.GetString(KeyCart) != null)
            {
                bienThes = JsonConvert.DeserializeObject<List<ChiTietSanPhamViewModelAdmin>>(HttpContext.Session.GetString(KeyCart));
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
                List<ChiTietSanPhamViewModel> bienThes;
                ChiTietSanPhamViewModel bienThe = JsonConvert.DeserializeObject<ChiTietSanPhamViewModel>(response.Content.ReadAsStringAsync().Result);
                string? result = HttpContext.Session.GetString(KeyCart);
                if (string.IsNullOrEmpty(result))
                {
                    bienThe.SoLuong = 1;
                    bienThes = new List<ChiTietSanPhamViewModel>() { bienThe };
                }
                else
                {
                    bienThes = JsonConvert.DeserializeObject<List<ChiTietSanPhamViewModel>>(result);
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
                HttpContext.Session.SetString("LoginInfor", response.Content.ReadAsStringAsync().Result);
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
        [HttpGet]
        public IActionResult Profile(string loginInfor)
        {
            LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(loginInfor);
            return View(loginViewModel);
        }
        public IActionResult ChangePassword()
        {
            return PartialView("ChangePassword");
        }
        public IActionResult PurchaseOrder()
        {

            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string newPassword)
        {
            var session = HttpContext.Session.GetString("LoginInfor");
            ChangePasswordRequest request = new ChangePasswordRequest();
            request.ID = JsonConvert.DeserializeObject<LoginViewModel>(session).Id;
            request.NewPassword = newPassword;
            var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "QuanLyNguoiDung/ChangePassword",request).Result;
            HttpContext.Session.Remove("LoginInfor");
            if(response.IsSuccessStatusCode) return RedirectToAction("Login");
            else return BadRequest();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult ForgotPassword()
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
        public ActionResult Pay(HoaDonViewModel hoaDon)
        {
            List<ChiTietHoaDonViewModel> lstChiTietHoaDon = new List<ChiTietHoaDonViewModel>();
            string temp = TempData["ListBienThe"] as string;
            foreach (var item in JsonConvert.DeserializeObject<List<ChiTietSanPhamViewModel>>(temp))
            {
                ChiTietHoaDonViewModel chiTietHoaDon = new ChiTietHoaDonViewModel();
                chiTietHoaDon.IDChiTietSanPham = item.ID;
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