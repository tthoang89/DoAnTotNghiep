using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.Mail;
using AppData.ViewModels.QLND;
using AppData.ViewModels.SanPham;
using AppData.ViewModels.VNPay;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web.Helpers;
using TechTalk.SpecFlow.Infrastructure;
using X.PagedList;


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
        public async Task<IActionResult> Index()
        {
            // lam start
            var session = HttpContext.Session.GetString("LoginInfor");
            if (String.IsNullOrEmpty(session))
            {
                List<GioHangRequest> lstGioHang = new List<GioHangRequest>();
                if (Request.Cookies["Cart"] != null)
                {
                    lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(Request.Cookies["Cart"]);
                }
                // laam them
                int cout = lstGioHang.Sum(c => c.SoLuong);
                TempData["SoLuong"] = cout.ToString();

                if (Request.Cookies["Cart"] != null)
                {
                    var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                    if (response.IsSuccessStatusCode)
                    {
                        var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                       

                        // lam end
                       
                        TempData["TrangThai"] = "false";
                        return View(temp.GioHangs);
                    }
                    else return BadRequest();
                }
                else
                {
                    TempData["TongTien"] = "0";
                    return View(new List<GioHangRequest>());
                }
            }
            else
            {
                var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                if (loginInfor.vaiTro == 1)
                {
                    var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                    if (response.IsSuccessStatusCode)
                    {
                        var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);


                        // lam them
                        int cout = temp.GioHangs.Sum(c => c.SoLuong);

                        TempData["SoLuong"] = cout.ToString();
                        // lam end
                        TempData["TrangThai"] = "true";
                        return View(temp.GioHangs);
                    }
                    else return BadRequest();
                }
                else {
                    TempData["SoLuong"] = "0";
                    return View(new List<GioHangRequest>());
                }
               
            }
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
        [HttpPost]
        public JsonResult ShowProduct(FilterData filter)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/getAll").Result;
            List<SanPhamViewModel> lstSanpham = new List<SanPhamViewModel>();
            if (response.IsSuccessStatusCode)
            {
                lstSanpham = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
                //
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
                        }
                        else if (item == "2")
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
                            if (lsttam1.FirstOrDefault(p => p.ID == item.ID) == null)
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
                    lstSanphamfn = lstSanphamfn.Where(p => p.Ten.ToLower().Contains(filter.search.ToLower())).ToList();
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
                        lstSanphamfn = lstSanphamfn.OrderBy(p => p.NgayTao.Value.Date).ThenBy(p => p.NgayTao.Value.TimeOfDay).ToList();
                    }
                    else if (filter.sortSP == "7")
                    {
                        lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.NgayTao.Value.Date).ThenBy(p => p.NgayTao.Value.TimeOfDay).ToList();
                    }
                    else if (filter.sortSP == "9")
                    {
                        lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.SoLuong).ToList();
                    }
                }
                List<SanPhamViewModel> lstSanPhamfnR = new List<SanPhamViewModel>();
                foreach (var item in lstSanphamfn)
                {
                    if (item.TrangThaiCTSP == 1)
                    {
                        if (lstSanPhamfnR.FirstOrDefault(p => p.ID == item.ID) == null)
                        {
                            lstSanPhamfnR.Add(item);
                        }
                    }
                    else
                    {
                        SanPhamViewModel sanPhamViewModel = lstSanpham.FirstOrDefault(p => p.ID == item.ID && p.TrangThaiCTSP == 1);
                        if (lstSanPhamfnR.FirstOrDefault(p => p.ID == sanPhamViewModel.ID) == null)
                        {
                            lstSanPhamfnR.Add(sanPhamViewModel);
                        }
                    }
                }
                //

                var model = lstSanPhamfnR.Skip((filter.page - 1) * filter.pageSize).Take(filter.pageSize).ToList();
                return Json(new
                {
                    data = model,
                    total = lstSanPhamfnR.Count,
                    status = true
                });
            }
            else return Json(new { status = false });
        }
        [HttpGet]
        public IActionResult Shop()
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
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetail(string idSanPham)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChiTietSanPhamHome?idSanPham=" + idSanPham);
            var chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamViewModelHome>(response.Content.ReadAsStringAsync().Result);
            return View(chiTietSanPham);
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetailFromCart(Guid idctsp)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetIDsanPhamByIdCTSP?idctsp=" + idctsp);
            var idsanpham = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
            return RedirectToAction("ProductDetail", "Home", new { idSanPham = idsanpham });
        }
        [HttpGet]
        public JsonResult ShowDanhGiabyIdSP(Guid id, int page, int pageSize)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"DanhGia/getbyIdSp/{id}").Result;
            List<DanhGiaViewModel> lstDanhGiaSanpham = new List<DanhGiaViewModel>();
            if (response.IsSuccessStatusCode)
            {
                lstDanhGiaSanpham = JsonConvert.DeserializeObject<List<DanhGiaViewModel>>(response.Content.ReadAsStringAsync().Result);
                var model = lstDanhGiaSanpham.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    data = model,
                    total = lstDanhGiaSanpham.Count,
                    status = true
                });
            }
            else return Json(new { status = false });
        }
        #endregion
        
        #region Filter
        //public IActionResult GetFilteredProducts([FromBody] FilterData filter)
        //{
        //    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/getAll").Result;
        //    List<SanPhamViewModel> lstSanpham = new List<SanPhamViewModel>();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        lstSanpham = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
        //    }
        //    List<SanPhamViewModel> lstSanphamfn = new List<SanPhamViewModel>();
        //    //price-filter
        //    if (filter.priceRange != null && filter.priceRange.Count > 0 && !filter.priceRange.Contains("all"))
        //    {
        //        foreach (var item in filter.priceRange)
        //        {
        //            if (item == "1")
        //            {
        //                foreach (var x in lstSanpham.Where(p => p.GiaBan < 100000).ToList())
        //                {
        //                    lstSanphamfn.Add(x);
        //                }
        //            }else if (item == "2")
        //            {
        //                foreach (var x in lstSanpham.Where(p => p.GiaBan >= 100000 && p.GiaBan < 200000).ToList())
        //                {
        //                    lstSanphamfn.Add(x);
        //                }
        //            }
        //            else if (item == "3")
        //            {
        //                foreach (var x in lstSanpham.Where(p => p.GiaBan >= 200000 && p.GiaBan < 300000).ToList())
        //                {
        //                    lstSanphamfn.Add(x);
        //                }
        //            }
        //            else if (item == "4")
        //            {
        //                foreach (var x in lstSanpham.Where(p => p.GiaBan >= 300000 && p.GiaBan < 400000).ToList())
        //                {
        //                    lstSanphamfn.Add(x);
        //                }
        //            }
        //            else if (item == "5")
        //            {
        //                foreach (var x in lstSanpham.Where(p => p.GiaBan >= 400000 && p.GiaBan < 500000).ToList())
        //                {
        //                    lstSanphamfn.Add(x);
        //                }
        //            }
        //            else if (item == "6")
        //            {
        //                foreach (var x in lstSanpham.Where(p => p.GiaBan > 500000).ToList())
        //                {
        //                    lstSanphamfn.Add(x);
        //                }
        //            }

        //        }
        //    }
        //    else
        //    {
        //        lstSanphamfn = lstSanpham;
        //    }
        //    //loaiSP-filter
        //    List<SanPhamViewModel> lsttam = new List<SanPhamViewModel>();
        //    List<SanPhamViewModel> lsttam1 = new List<SanPhamViewModel>();
        //    if (filter.loaiSP != null && filter.loaiSP.Count > 0)
        //    {
        //        foreach (var x in filter.loaiSP)
        //        {
        //            lsttam = lstSanphamfn.Where(p => p.LoaiSP == x).ToList();
        //            foreach (var item in lsttam)
        //            {
        //                if (lsttam1.FirstOrDefault(p=>p.ID == item.ID) == null)
        //                {
        //                    lsttam1.Add(item);
        //                }
        //            }
        //        }
        //        lstSanphamfn = lsttam1;
        //    }
        //    //Search
        //    if (filter.search != null)
        //    {
        //        lstSanphamfn = lstSanphamfn.Where(p=>p.Ten.ToLower().Contains(filter.search.ToLower())).ToList();
        //    }

        //    //color-filter
        //    List<SanPhamViewModel> lstmautam = new List<SanPhamViewModel>();
        //    List<SanPhamViewModel> lstmautam1 = new List<SanPhamViewModel>();
        //    if (filter.mauSac != null && filter.mauSac.Count > 0)
        //    {
        //        foreach (var x in filter.mauSac)
        //        {
        //            lstmautam = lstSanphamfn.Where(p => p.IDMauSac == x).ToList();
        //            foreach (var item in lstmautam)
        //            {
        //                if (lstmautam1.FirstOrDefault(p => p.ID == item.ID) == null)
        //                {
        //                    lstmautam1.Add(item);
        //                }
        //            }
        //        }
        //        lstSanphamfn = lstmautam1;
        //    }
        //    //size-filter
        //    List<SanPhamViewModel> lstcotam = new List<SanPhamViewModel>();
        //    List<SanPhamViewModel> lstcotam1 = new List<SanPhamViewModel>();
        //    if (filter.kichCo != null && filter.kichCo.Count > 0)
        //    {
        //        foreach (var x in filter.kichCo)
        //        {
        //            lstcotam = lstSanphamfn.Where(p => p.IDKichCo == x).ToList();
        //            foreach (var item in lstcotam)
        //            {
        //                if (lstcotam1.FirstOrDefault(p => p.ID == item.ID) == null)
        //                {
        //                    lstcotam1.Add(item);
        //                }
        //            }
        //        }
        //        lstSanphamfn = lstcotam1;
        //    }
        //    //material-filter
        //    List<SanPhamViewModel> lstchatlieutam = new List<SanPhamViewModel>();
        //    List<SanPhamViewModel> lstchatlieutam1 = new List<SanPhamViewModel>();
        //    if (filter.chatLieu != null && filter.chatLieu.Count > 0)
        //    {
        //        foreach (var x in filter.chatLieu)
        //        {
        //            lstchatlieutam = lstSanphamfn.Where(p => p.IDChatLieu == x).ToList();
        //            foreach (var item in lstchatlieutam)
        //            {
        //                if (lstchatlieutam1.FirstOrDefault(p => p.ID == item.ID) == null)
        //                {
        //                    lstchatlieutam1.Add(item);
        //                }
        //            }
        //        }
        //        lstSanphamfn = lstchatlieutam1;
        //    }
        //    //sort
        //    if (filter.sortSP != null)
        //    {
        //        if (filter.sortSP == "2")
        //        {
        //            lstSanphamfn = lstSanphamfn.OrderBy(p => p.GiaBan).ToList();
        //        }
        //        else if (filter.sortSP == "3")
        //        {
        //            lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.GiaBan).ToList();
        //        }
        //        else if (filter.sortSP == "4")
        //        {
        //            lstSanphamfn = lstSanphamfn.OrderBy(p => p.GiaBan).ToList();
        //        }
        //        else if (filter.sortSP == "5")
        //        {
        //            lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.GiaBan).ToList();
        //        }
        //        else if (filter.sortSP == "6")
        //        {
        //            lstSanphamfn = lstSanphamfn.OrderBy(p => p.NgayTao).ToList();
        //        }
        //        else if (filter.sortSP == "7")
        //        {
        //            lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.NgayTao).ToList();
        //        }
        //        else if (filter.sortSP == "9")
        //        {
        //            lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.SoLuong).ToList();
        //        }
        //    }
        //    List<SanPhamViewModel>lstSanPhamfnR = new List<SanPhamViewModel>();
        //    foreach (var item in lstSanphamfn)
        //    {
        //        if (item.TrangThaiCTSP == 1)
        //        {
        //            if (lstSanPhamfnR.FirstOrDefault(p => p.ID == item.ID) == null)
        //            {
        //                lstSanPhamfnR.Add(item);
        //            }

        //        }
        //        else
        //        {
        //            SanPhamViewModel sanPhamViewModel = lstSanpham.FirstOrDefault(p=>p.ID == item.ID && p.TrangThaiCTSP == 1);
        //            if (lstSanPhamfnR.FirstOrDefault(p=>p.ID == sanPhamViewModel.ID) == null)
        //            {
        //                lstSanPhamfnR.Add(sanPhamViewModel);
        //            }
        //        }
        //    }

        //    return PartialView("_ReturnProducts", lstSanPhamfnR);
        //}
        #endregion
        [HttpPost]
        public async Task<ActionResult> TongSoLuong(int? sl)
        {
            var session = HttpContext.Session.GetString("LoginInfor");
            if (String.IsNullOrEmpty(session))
            {
                List<GioHangRequest> lstGioHang = new List<GioHangRequest>();
                if (Request.Cookies["Cart"] != null)
                {
                    lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(Request.Cookies["Cart"]);
                }
                // laam them
                int cout = lstGioHang.Sum(c => c.SoLuong)+sl.Value;
                TempData["SoLuong"] = cout.ToString();

                if (Request.Cookies["Cart"] != null)
                {
                    var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                    if (response.IsSuccessStatusCode)
                    {
                        var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result); 
                        cout = temp.GioHangs.Sum(c => c.SoLuong) + sl.Value;
                        TempData["SoLuong"] = cout.ToString();
                        // lam end

                        return Json(new { success = true, message = "Add to cart successfully",sl=cout });
                    }
                    else return Json(new { error = true, message = "  Not Add to cart " });
                }
                else
                {
                    TempData["SoLuong"] = "0";
                    return View(new List<GioHangRequest>());
                }
            }
            else
            {
                var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                if (loginInfor.vaiTro == 1)
                {
                    var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                    if (response.IsSuccessStatusCode)
                    {
                        var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);

                        // lam them
                        int cout = temp.GioHangs.Sum(c => c.SoLuong) + sl.Value;

                        TempData["SoLuong"] = cout.ToString();
                        // lam end
                        TempData["TrangThai"] = "true";
                        return Json(new { success = true, message = "Add to cart successfully", sl = cout });
                    }
                    else return BadRequest();
                }
                else 
                {
                    TempData["SoLuong"] = "0";
                    return Json(new { error = false, message = "  Not Add to cart " }); ;
                } 
            }
        }
        #region Cart
        [HttpGet]
        public async Task<IActionResult> ShoppingCart()
        {
            var session = HttpContext.Session.GetString("LoginInfor");
            if (String.IsNullOrEmpty(session))
            {
                List<GioHangRequest> lstGioHang = new List<GioHangRequest>();
                if (Request.Cookies["Cart"] != null)
                {
                    lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(Request.Cookies["Cart"]);
                }
                // laam them
                int cout = lstGioHang.Sum(c => c.SoLuong);              
                TempData["SoLuong"] = cout.ToString();

                if (Request.Cookies["Cart"] != null)
                {
                    var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                    if (response.IsSuccessStatusCode)
                    {
                        var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                        TempData["TongTien"] = temp.TongTien.ToString();
                       
                        // lam end
                        TempData["ListBienThe"] = JsonConvert.SerializeObject(temp.GioHangs);
                       
                        TempData["TrangThai"] = "false";
                        return View(temp.GioHangs);
                    }
                    else return BadRequest();
                }
                else {
                    TempData["TongTien"] = "0";
                    return View(new List<GioHangRequest>());
                } 
            }
            else
            {
                var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                if (loginInfor.vaiTro == 1)
                {
                    var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                    if (response.IsSuccessStatusCode)
                    {
                        var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                        TempData["TongTien"] = temp.TongTien.ToString();
                        TempData["ListBienThe"] = JsonConvert.SerializeObject(temp.GioHangs);
                        // lam them
                        int cout = temp.GioHangs.Sum(c => c.SoLuong);

                        TempData["SoLuong"] = cout.ToString();
                        // lam end
                        TempData["TrangThai"] = "true";
                        return View(temp.GioHangs);
                    }
                    else return BadRequest();
                }
                else
                {
                    TempData["SoLuong"] = "0";
                    return View(new List<GioHangRequest>());
                }
                }
            }
        [HttpGet]
        public ActionResult DeleteFromCart(Guid id)
        {
            List<GioHangRequest> bienThes = new List<GioHangRequest>();
            string? result = Request.Cookies["Cart"];
            if (result != null)
            {
                bienThes = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
            }
            bienThes.Remove(bienThes.Find(p => p.IDChiTietSanPham == id));
            //
            var session = HttpContext.Session.GetString("LoginInfor");
            if (session == null)
            {
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Append("Cart", JsonConvert.SerializeObject(bienThes), cookie);
                return RedirectToAction("ShoppingCart");
            }
            else
            {
                var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                if (loginInfor.vaiTro == 1)
                {
                    var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + $"GioHang/Deletebyid?idctsp={id}&idNguoiDung={loginInfor.Id}").Result;
                    if (response.IsSuccessStatusCode) return RedirectToAction("ShoppingCart");
                    else return RedirectToAction("ShoppingCart");
                }
                else return RedirectToAction("ShoppingCart");
            }
            //
            
        }
        [HttpPost]
        public ActionResult AddToCart(string id, int? sl)
        {
            List<GioHangRequest> lstGioHang;
            string? result = Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(result))
            {
                //chiTietSanPham.SoLuong = (sl != null)?sl.Value:1;
                lstGioHang = new List<GioHangRequest>() { new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (sl != null) ? sl.Value : 1 } };
            }
            else
            {
                lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                var tempBienThe = lstGioHang.FirstOrDefault(x => x.IDChiTietSanPham == new Guid(id));
                if (tempBienThe != null)
                {
                    //Sua 
                    if (sl == null)
                    {
                        tempBienThe.SoLuong++;
                    }
                    else
                    {
                        tempBienThe.SoLuong = tempBienThe.SoLuong + sl.Value;
                    }

                }
                else
                {
                    lstGioHang.Add(new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (sl != null) ? sl.Value : 1 });
                }
            }
            var session = HttpContext.Session.GetString("LoginInfor");
            if(session == null)
            {
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Append("Cart", JsonConvert.SerializeObject(lstGioHang), cookie);
               
                return Json(new { success = true, message = "Add to cart successfully" });
            }
            else
            {
                var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                if (loginInfor.vaiTro == 1)
                {
                    var chiTietGioHang = new ChiTietGioHang() { ID = Guid.NewGuid(), SoLuong = (sl != null) ? sl.Value : 1, IDCTSP = new Guid(id), IDNguoiDung = loginInfor.Id };
                    var response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "GioHang/AddCart",chiTietGioHang).Result;
                   
                    if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Add to cart successfully" });
                    else return Json(new { success = false, message = "Add to cart fail" });
                }
                else return Json(new { success = false, message = "Chỉ khách hàng mới thêm được vào giỏ hàng" });
            }
        }
        [HttpPost]
        public IActionResult UpdateCart(List<string> dssl)
        {
            try
            {
                List<GioHangRequest> chiTietSanPhams;
                string? result = Request.Cookies["Cart"];
                chiTietSanPhams = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                foreach (var item in dssl)
                {
                    Guid id = Guid.Parse(item.Substring(0, 36));
                    int sl = Convert.ToInt32(item.Substring(36, item.Length - 36));
                    foreach (var x in chiTietSanPhams)
                    {
                        if (x.IDChiTietSanPham == id)
                        {
                            x.SoLuong = sl;
                        }
                    }
                }
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session == null)
                {
                    CookieOptions cookie = new CookieOptions();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append("Cart", JsonConvert.SerializeObject(chiTietSanPhams), cookie);
                    return Json(new { success = true, message = "Cập nhật giỏ hàng thành công" });
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        foreach (var item in dssl)
                        {
                            Guid id = Guid.Parse(item.Substring(0, 36));
                            int sl = Convert.ToInt32(item.Substring(36, item.Length - 36));
                            var response = _httpClient.PutAsync(_httpClient.BaseAddress + $"GioHang/UpdateCart?idctsp={id}&soluong={sl}&idNguoiDung={loginInfor.Id}", null).Result;
                        }
                        return Json(new { success = true, message = "Cập nhật giỏ hàng thành công" });
                    }
                    else return Json(new { success = false, message = "Chỉ khách hàng mới thêm được vào giỏ hàng" });
                }
                
            }
            catch (Exception)
            {

                return Json(new { success = true, message = "Cập nhật giỏ hàng thất bại" });
            }

        }
        [HttpPost]
        public ActionResult BuyNow(string id, int soLuong)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"SanPham/GetChiTietSanPhamByID?id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                //moiw
                List<GioHangRequest> lstGioHang;
                //ChiTietSanPhamViewModel chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamViewModel>(response.Content.ReadAsStringAsync().Result);
                string? result = Request.Cookies["Cart"];
                if (string.IsNullOrEmpty(result))
                {
                    //chiTietSanPham.SoLuong = (sl != null)?sl.Value:1;
                    lstGioHang = new List<GioHangRequest>() { new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (soLuong != null) ? soLuong : 1 } };
                }
                else
                {
                    lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                    var tempBienThe = lstGioHang.FirstOrDefault(x => x.IDChiTietSanPham == new Guid(id));
                    if (tempBienThe != null)
                    {
                        //Sua 
                        if (soLuong == null)
                        {
                            tempBienThe.SoLuong++;
                        }
                        else
                        {
                            tempBienThe.SoLuong = tempBienThe.SoLuong + soLuong;
                        }

                    }
                    else
                    {
                        //chiTietSanPham.SoLuong = (sl != null) ? sl.Value : 1;
                        lstGioHang.Add(new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (soLuong != null) ? soLuong : 1 });
                    }
                }
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session == null)
                {
                    CookieOptions cookie = new CookieOptions();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append("Cart", JsonConvert.SerializeObject(lstGioHang), cookie);
                    return Json(new { success = true, message = "Add to cart successfully" });
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var chiTietGioHang = new ChiTietGioHang() { ID = Guid.NewGuid(), SoLuong = (soLuong != null) ? soLuong : 1, IDCTSP = new Guid(id), IDNguoiDung = loginInfor.Id };
                        var response1 = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "GioHang/AddCart", chiTietGioHang).Result;
                        if (response1.IsSuccessStatusCode) return Json(new { success = true, message = "Add to cart successfully" });
                        else return Json(new { success = false, message = "Add to cart fail" });
                    }
                    else return Json(new { success = false, message = "Chỉ khách hàng mới thêm được vào giỏ hàng" });
                }

            }
            else return Json(new { success = false, message = "Add to cart fail" });
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login(string actionName)
        {
            TempData["ActionName"] = actionName;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(string login, string password)
        {
            //https://localhost:7095/api/QuanLyNguoiDung/DangNhap?email=tam%40gmail.com&password=chungtam200396
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin đăng nhập.";
                return View();
            }
            if (login.Contains('@'))
            {
                login = login.Replace("@", "%40");
            }
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangNhap?lg={login}&password={password}").Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("LoginInfor", response.Content.ReadAsStringAsync().Result);
                string actionName = TempData["ActionName"].ToString();
                return RedirectToAction(actionName);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.ErrorMessage = "Bạn không có quyền truy cập vào tài khoản này.";
                return View();
            }
            else
            {
                ViewBag.ErrorMessage = "Email hoặc password không chính xác.";
                return View();
            }
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
        //public IActionResult ChangePassword()
        //{
        //    return PartialView("ChangePassword");
        //}
        [HttpPut]
        public ActionResult UpdateProfile(string ten,string email,string sdt,int? gioitinh,DateTime? ngaysinh,string? diachi)
        {
            var session = HttpContext.Session.GetString("LoginInfor");
            LoginViewModel khachhang = new LoginViewModel();
            khachhang.Id = JsonConvert.DeserializeObject<LoginViewModel>(session).Id;
            khachhang.Ten = ten;
            khachhang.Email = email;
            khachhang.SDT = sdt;
            khachhang.GioiTinh = gioitinh;
            khachhang.NgaySinh = ngaysinh;
            khachhang.DiaChi = diachi;
            khachhang.DiemTich = JsonConvert.DeserializeObject<LoginViewModel>(session).DiemTich;
            khachhang.vaiTro = JsonConvert.DeserializeObject<LoginViewModel>(session).vaiTro;
            var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "QuanLyNguoiDung/UpdateProfile", khachhang).Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("LoginInfor", response.Content.ReadAsStringAsync().Result);
                return Json(new { success = true, message = "Cập nhật thông tin cá nhân thành công" });
            }
            else
            {
                return Json(new { success = false, message = "Cập nhật thông tin cá nhân thất bại" });
            }
        }
        public IActionResult PurchaseOrder()
        {
            var session = HttpContext.Session.GetString("LoginInfor");
            LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
            List<DonMuaViewModel> donMuaViewModels = new List<DonMuaViewModel>();
            //HttpResponseMessage responseDonMua = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetAllDonMua?IDkhachHang={loginViewModel.Id}").Result;
            //if (responseDonMua.IsSuccessStatusCode)
            //{
            //    donMuaViewModels = JsonConvert.DeserializeObject<List<DonMuaViewModel>>(responseDonMua.Content.ReadAsStringAsync().Result);
            //}
            return View("PurchaseOrder", donMuaViewModels);
        }
        public IActionResult PurchaseOrderDetail(Guid idHoaDon)
        {
            List<DonMuaChiTietViewModel> DonMuaCT = new List<DonMuaChiTietViewModel>();
            HttpResponseMessage responseDonMuaCT = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetAllDonMuaChiTiet?idHoaDon={idHoaDon}").Result;
            if (responseDonMuaCT.IsSuccessStatusCode)
            {
                DonMuaCT = JsonConvert.DeserializeObject<List<DonMuaChiTietViewModel>>(responseDonMuaCT.Content.ReadAsStringAsync().Result);
            }
            return View("PurchaseOrderDetail", DonMuaCT);
        }
        public IActionResult ReviewProducts(Guid idCTHD)
        {
            ChiTietHoaDonDanhGiaViewModel hdctDanhGia = new ChiTietHoaDonDanhGiaViewModel();
            HttpResponseMessage responseDonMuaCT = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetCTHDDANHGIA?idhdct={idCTHD}").Result;
            if (responseDonMuaCT.IsSuccessStatusCode)
            {
                hdctDanhGia = JsonConvert.DeserializeObject<ChiTietHoaDonDanhGiaViewModel>(responseDonMuaCT.Content.ReadAsStringAsync().Result);
            }
            return View("ReviewProducts", hdctDanhGia);
        }
        [HttpPost]
        public IActionResult ChangePassword(string newPassword, string oldPassword)
        {
            var session = HttpContext.Session.GetString("LoginInfor");
            ChangePasswordRequest request = new ChangePasswordRequest();
            request.ID = JsonConvert.DeserializeObject<LoginViewModel>(session).Id;
            request.NewPassword = newPassword;
            request.OldPassword = oldPassword;
            var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "QuanLyNguoiDung/ChangePassword", request).Result;
            HttpContext.Session.Remove("LoginInfor");
            if (response.IsSuccessStatusCode) return RedirectToAction("Login");
            else return BadRequest();
        }
        public IActionResult DanhGiaSanPham([FromBody] DanhGiaCTHDViewModel danhGiaCTHDView)
        {
            if (danhGiaCTHDView.danhgia == "")
            {
                danhGiaCTHDView.danhgia = "Người dùng này không để lại bình luận";
            }
            HttpResponseMessage responseDonMuaCT = _httpClient.PutAsync(_httpClient.BaseAddress + $"DanhGia?idCTHD={danhGiaCTHDView.idCTHD}&soSao={danhGiaCTHDView.soSao}&binhLuan={danhGiaCTHDView.danhgia}", null).Result;
            if (responseDonMuaCT.IsSuccessStatusCode)
            {
                RedirectToAction("PurchaseOrderDetail", danhGiaCTHDView.idHD);
            }
            return RedirectToAction("PurchaseOrderDetail", danhGiaCTHDView.idHD);
        }
        public IActionResult HuyDonHang(Guid idHoaDon)
        {
            HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=8", null).Result;
            if (responseDonMua.IsSuccessStatusCode)
            {
                RedirectToAction("PurchaseOrder");
            }
            return RedirectToAction("PurchaseOrder");
        }
        public IActionResult HoanTacHuyDonHang(Guid idHoaDon)
        {
            HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=2", null).Result;
            if (responseDonMua.IsSuccessStatusCode)
            {
                RedirectToAction("PurchaseOrder");
            }
            return RedirectToAction("PurchaseOrder");
        }
        public IActionResult DoiTraHang(Guid idHoaDon)
        {
            HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=9", null).Result;
            if (responseDonMua.IsSuccessStatusCode)
            {
                RedirectToAction("PurchaseOrder");
            }
            return RedirectToAction("PurchaseOrder");
        }
        public IActionResult HoanTacDoiTraHang(Guid idHoaDon)
        {
            HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=6", null).Result;
            if (responseDonMua.IsSuccessStatusCode)
            {
                RedirectToAction("PurchaseOrder");
            }
            return RedirectToAction("PurchaseOrder");
        }
        public IActionResult XacNhanGHTC(Guid idHoaDon)
        {
            HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=6", null).Result;
            if (responseDonMua.IsSuccessStatusCode)
            {
                RedirectToAction("PurchaseOrderDetail",idHoaDon);
            }
            return RedirectToAction("PurchaseOrder");
        }
        public IActionResult GetHoaDonByTrangThai(HoaDon danhGiaCTHDView, int page, int pageSize, string Search)
        {
            var session = HttpContext.Session.GetString("LoginInfor");
            LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
            List<DonMuaViewModel> donMuaViewModels = new List<DonMuaViewModel>();
            HttpResponseMessage responseDonMua = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetAllDonMua?IDkhachHang={loginViewModel.Id}").Result;
            if (responseDonMua.IsSuccessStatusCode)
            {
                donMuaViewModels = JsonConvert.DeserializeObject<List<DonMuaViewModel>>(responseDonMua.Content.ReadAsStringAsync().Result);
                donMuaViewModels = donMuaViewModels.OrderByDescending(p => p.NgayTao).ToList();

                foreach (var item in donMuaViewModels)
                {
                    item.Ngaytao1 = item.NgayTao.ToString("dd/MM/yyyy");
                    item.Ngaythanhtoan1 = item.NgayThanhToan != null ? item.NgayThanhToan.Value.ToString("dd/MM/yyyy"): null;
                    item.Ngaynhanhang1 = item.NgayNhanHang != null? item.NgayNhanHang.Value.ToString("dd/MM/yyyy"):null;
                }
                if (Search != null)
                {
                    donMuaViewModels = donMuaViewModels.Where(p => p.MaHD.ToLower().Contains(Search.ToLower())).ToList();
                }
                if (danhGiaCTHDView.TrangThaiGiaoHang != 0 && danhGiaCTHDView.TrangThaiGiaoHang != null)
                {
                    donMuaViewModels = donMuaViewModels.Where(p => p.TrangThaiGiaoHang == danhGiaCTHDView.TrangThaiGiaoHang).ToList();

                    var model = donMuaViewModels.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return Json(new
                    {
                        data = model,
                        total = donMuaViewModels.Count,
                        status = true
                    });
                }
                else
                {
                    var model = donMuaViewModels.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return Json(new
                    {
                        data = model,
                        total = donMuaViewModels.Count,
                        status = true
                    });
                }
            }
            else return Json(new { status = false });

        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet]

        #endregion

        #region ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        //{
        //    try
        //    {
        //        string apiUrl = $"https://localhost:7095/api/QuanLyNguoiDung/ForgotPassword?request={request.Email}";
        //        var response = await _httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Xử lý phản hồi từ controller
        //            var result = await response.Content.ReadAsStringAsync();
        //            if (result == "EmailExists")
        //            {
        //                TempData["Message"] = "Vui lòng kiểm tra email của bạn .";
        //                return RedirectToAction("Login"); 
        //            }
        //            else if (result == "Email này không tồn tại")
        //            {
        //                TempData["Message"] = "Email này không tồn tại.";
        //            }
        //            else
        //            {
        //                TempData["Message"] = "Không thể gửi lại email. Vui lòng thử lại sau..";
        //            }
        //        }
        //        else
        //        {
        //            // Xử lý khi controller trả về lỗi
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            ModelState.AddModelError("", errorResponse);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý khi có lỗi trong quá trình gửi yêu cầu đến controller
        //        Console.WriteLine(ex.Message);
        //        ModelState.AddModelError("", "Không thể gửi lại email. Vui lòng thử lại sau.");
        //    }
        //    return View();
        //}
        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var khachHang = JsonConvert.DeserializeObject<KhachHangViewModel>(_httpClient.GetAsync(_httpClient.BaseAddress + "KhachHang/GetKhachHangByEmail?email=" + email).Result.Content.ReadAsStringAsync().Result);
            if (khachHang.Id != null)
            {
                string ma = Guid.NewGuid().ToString().Substring(0, 5);
                MailData mailData = new MailData() { EmailToId = email, EmailToName = email, EmailSubject = "Mã xác nhận", EmailBody = "Mã xác nhận: " + ma };
                HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "Mail/SendMail", mailData).Result;
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("ForgotPassword", khachHang.Id + ":" + ma);
                    return RedirectToAction("SubmitForgotPassword");
                }
                else return BadRequest();
            }
            else return BadRequest("Không tìm thấy email");
        }
        [HttpGet]
        public IActionResult SubmitForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitForgotPassword(string code)
        {
            string[] submit = HttpContext.Session.GetString("ForgotPassword").Split(":");
            if (code == submit[1])
            {
                return RedirectToAction("ChangeForgotPassword");
            }
            else return BadRequest("Mã không hợp lệ");
        }
        [HttpGet]
        public IActionResult ChangeForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeForgotPassword(string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(password))
            {
                ViewData["PasswordError"] = "Mật khẩu không được bỏ trống";
                return View();
            }
            else if (password.Length < 8)
            {
                ViewData["PasswordError"] = "Mật khẩu phải lớn hơn 8 ký tự";
                return View();
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                ViewData["ConfirmPasswordError"] = "Xác nhận mật khẩu không được bỏ trống";
                return View();
            }
            else if (password != confirmPassword)
            {
                ViewData["ConfirmPasswordError"] = "Mật khẩu và xác nhận mật khẩu không khớp";
                return View();
            }

            if (password == confirmPassword)
            {
                string[] submit = HttpContext.Session.GetString("ForgotPassword").Split(":");
                KhachHangViewModel khachHang = new KhachHangViewModel() { Id = new Guid(submit[0]), Password = password };
                HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "KhachHang/ChangeForgotPassword", khachHang).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Không thể đặt lại mật khẩu. Vui lòng kiểm tra lại thông tin.";
                    return View();
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Không thể đặt lại mật khẩu. Vui lòng kiểm tra lại thông tin.";
                return View();
            }

        }


        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            // Kiểm tra email và chuyển đến trang ResetPassword nếu email hợp lệ
            if (!string.IsNullOrEmpty(email))
            {
                ResetPasswordRequest request = new ResetPasswordRequest
                {
                    Email = email
                };
                return View(request);
            }
            else
            {
                // Xử lý khi email không hợp lệ
                TempData["Message"] = "Invalid email.";
                return RedirectToAction("Login");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {

            if (ModelState.IsValid)
            {
                // Validate password and confirm password match
                if (request.Password != request.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match");
                    return View("ResetPassword");
                }

                // Implement the code to save the new password in your QuanLyNguoiDungService
                string apiUrl = "https://localhost:7095/api/QuanLyNguoiDung/ResetPassword";
                var response = await _httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Password reset successfully";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Không thể đặt lại mật khẩu");
                }
            }

            return View("ResetPassword");
        }

        #endregion

        #region CheckOut
        [HttpGet]
        public JsonResult UseDiemTich(int diem, string id, int tongTien)
        {
            var response = _httpClient.GetAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/UseDiemTich?diem={diem}&id={id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var soTienGiam = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                if (soTienGiam < (tongTien/2))
                {
                    return Json(new { SoTienGiam = soTienGiam, TrangThai = true });
                }
                else
                {
                    return Json(new { Loi = "Số tiền giảm khi sử dụng điểm phải nhỏ hơn 50% giá trị đơn hàng", TrangThai = false });
                }
            }
            else return Json(new {Loi = "Không kết nối được với server",TrangThai = false});
        }
        [HttpGet]
        public IActionResult CheckOut()
        {
            return View();
        }
        [HttpPost]
        public string Order(HoaDonViewModel hoaDon)
        {
            if (hoaDon.PhuongThucThanhToan == "COD")
            {
                List<ChiTietHoaDonViewModel> lstChiTietHoaDon = new List<ChiTietHoaDonViewModel>();
                string temp = TempData["ListBienThe"] as string;
                string trangThai = TempData["TrangThai"] as string;
                foreach (var item in JsonConvert.DeserializeObject<List<GioHangRequest>>(temp))
                {
                    ChiTietHoaDonViewModel chiTietHoaDon = new ChiTietHoaDonViewModel();
                    chiTietHoaDon.IDChiTietSanPham = item.IDChiTietSanPham;
                    chiTietHoaDon.SoLuong = item.SoLuong;
                    chiTietHoaDon.DonGia = item.DonGia.Value;
                    lstChiTietHoaDon.Add(chiTietHoaDon);
                }
                hoaDon.ChiTietHoaDons = lstChiTietHoaDon;
                hoaDon.TrangThai = Convert.ToBoolean(trangThai);
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    hoaDon.IDNguoiDung = JsonConvert.DeserializeObject<LoginViewModel>(session).Id;
                }
                TempData.Remove("TongTien");
                HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
                if (response.IsSuccessStatusCode)
                {
                  
                    if(!hoaDon.TrangThai) Response.Cookies.Delete("Cart");
                    // lam them
                    TempData["SoLuong"] = "0";
                    // lam end
                    return "https://localhost:5001/Home/CheckOutSuccess";
                }
                else return "";
            }
            else if (hoaDon.PhuongThucThanhToan == "VNPay")
            {
                TempData["HoaDon"] = JsonConvert.SerializeObject(hoaDon);
                string vnp_Returnurl = "https://localhost:5001/Home/PaymentCallBack"; //URL nhan ket qua tra ve 
                string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = "OFZ9Q6W4"; //Ma định danh merchant kết nối (Terminal Id)
                string vnp_HashSecret = "IKQOFVXJPGYEIDNVNICTIIFPXNTXRYCX"; //Secret Key
                string ipAddr = HttpContext.Connection.RemoteIpAddress?.ToString();
                //Get payment input
                OrderInfo order = new OrderInfo();
                order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
                order.Amount = hoaDon.TongTien;
                order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
                order.CreatedDate = DateTime.Now;
                //Save order to db

                //Build URL for VNPAY
                VnPayLibrary vnpay = new VnPayLibrary();

                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", "OFZ9Q6W4");
                vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
                vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", ipAddr);
                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
                vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

                //Add Params of 2.1.0 Version
                //Billing

                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, "IKQOFVXJPGYEIDNVNICTIIFPXNTXRYCX");
                //log.InfoFormat("VNPAY URL: {0}", paymentUrl);

                return paymentUrl;
            }
            else return "";
        }
        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            if (Request.Query.Count > 0)
            {
                string vnp_HashSecret = "IKQOFVXJPGYEIDNVNICTIIFPXNTXRYCX"; //Chuoi bi mat
                var vnpayData = Request.Query;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (var s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s.Key, vnpayData[s.Key]);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.Query["vnp_SecureHash"];
                String TerminalID = Request.Query["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.Query["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        var hoaDon = JsonConvert.DeserializeObject<HoaDonViewModel>(TempData["HoaDon"].ToString());
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
                        hoaDon.Diem = 0;
                        hoaDon.NgayThanhToan = DateTime.Now;
                        HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Response.Cookies.Delete("Cart");
                            return RedirectToAction("CheckOutSuccess");
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else return BadRequest();
        }
        [HttpGet]
        public IActionResult CheckOutSuccess()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> UseVoucher(string ma, int tongTien)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "Voucher/GetVoucherByMa?ma=" + ma);
            if (response.IsSuccessStatusCode)
            {
                var voucher = JsonConvert.DeserializeObject<Voucher>(await response.Content.ReadAsStringAsync());
                if (voucher != null)
                {
                    if(voucher.NgayKetThuc>DateTime.Now && voucher.NgayApDung < DateTime.Now)
                    {
                        if (voucher.SoLuong > 0)
                        {
                            if (voucher.SoTienCan < tongTien)
                            {
                                return Json(new { HinhThuc = voucher.HinhThucGiamGia, GiaTri = voucher.GiaTri, TrangThai = true });
                            }
                            else
                            {
                                return Json(new { Loi = "Voucher chưa đạt đủ điều kiện: Tổng tiền sản phẩm lớn hơn "+voucher.SoTienCan.ToString("n0")+" VNĐ", TrangThai = false });
                            }
                        }
                        else
                        {
                            return Json(new { Loi = "Voucher đã sử dụng hết", TrangThai = false });
                        }
                    }
                    else
                    {
                        return Json(new { Loi = "Mã voucher hết hạn", TrangThai = false });
                    }
                }
                else
                {
                    return Json(new { Loi = "Không tìm thấy voucher", TrangThai=false });
                }
            }
            else return Json(new { HinhThuc = false, GiaTri = 0 });
        }
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
        #endregion
    }
}