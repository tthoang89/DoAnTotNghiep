using AppData.Models;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        public AdminController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        public IActionResult HomePageAdmin(Guid id)
        {
            return View();
        }
        public IActionResult ProductManager()
        {
            var response = _httpClient.GetAsync(_httpClient.BaseAddress+ "SanPham/getAll").Result;
            var lstSanPham = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
            return View(lstSanPham);
        }
        public JsonResult GetLoaiSPCha()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllLoaiSPCha").Result;
            var loaiSP = JsonConvert.DeserializeObject<List<LoaiSP>>(response.Content.ReadAsStringAsync().Result);
            return Json(loaiSP);
        }
        public JsonResult GetAllMauSac()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllMauSac").Result;
            var mauSac = JsonConvert.DeserializeObject<List<MauSac>>(response.Content.ReadAsStringAsync().Result);
            return Json(mauSac);
        }
        public JsonResult GetAllKichCo()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllKichCo").Result;
            var kichCo = JsonConvert.DeserializeObject<List<KichCo>>(response.Content.ReadAsStringAsync().Result);
            return Json(kichCo);
        }
        public JsonResult GetAllChatLieu()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChatLieu").Result;
            var chatLieu = JsonConvert.DeserializeObject<List<ChatLieu>>(response.Content.ReadAsStringAsync().Result);
            return Json(chatLieu);
        }
        public JsonResult GetLoaiSPCon(string tenLoaiSPCha)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllLoaiSPCon?tenLoaiSPCha=" + tenLoaiSPCha).Result;
            var loaiSP = JsonConvert.DeserializeObject<List<LoaiSP>>(response.Content.ReadAsStringAsync().Result);
            return Json(loaiSP);
        }
        [HttpGet]
        public IActionResult AddSanPham()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSanPham(SanPhamRequest sanPhamRequest)
        {
            HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddSanPham", sanPhamRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductManager");
            }
            else return BadRequest();
        }
        [HttpGet]
        public IActionResult ProductDetail(string idSanPham)
        {
            var response = _httpClient.GetAsync(_httpClient.BaseAddress+ "SanPham/GetAllChiTietSanPham?idSanPham="+idSanPham).Result;
            var lstSanPham = JsonConvert.DeserializeObject<List<ChiTietSanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
            return View(lstSanPham);
        }
    }
}
