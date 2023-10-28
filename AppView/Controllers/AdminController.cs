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
            return View();
        }
        //[HttpPost]
        //public IActionResult ProductManager()
        //{
        //    return View();
        //}
        public JsonResult GetLoaiSPCha()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "LoaiSP/GetLoaiSPCha").Result;
            var loaiSP = JsonConvert.DeserializeObject<List<LoaiSP>>(response.Content.ReadAsStringAsync().Result);
            return Json(loaiSP);
        }
        public JsonResult GetLoaiSPCon(Guid idLoaiSPCha)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "LoaiSP/GetLoaiSPCon?idLoaiSPCha=" + idLoaiSPCha).Result;
            var loaiSP = JsonConvert.DeserializeObject<List<LoaiSP>>(response.Content.ReadAsStringAsync().Result);
            return Json(loaiSP);
        }
        public JsonResult GetThuocTinh()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "ThuocTinh/getall").Result;
            var thuocTinhs = JsonConvert.DeserializeObject<List<ThuocTinhRequest>>(response.Content.ReadAsStringAsync().Result);
            return Json(thuocTinhs);
        }
        public JsonResult GetGiaTri(string thuocTinh)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "ThuocTinh/GetGiaTri?thuocTinh=" + thuocTinh).Result;
            var giaTris = JsonConvert.DeserializeObject<List<GiaTri>>(response.Content.ReadAsStringAsync().Result);
            return Json(giaTris);
        }
        
    }
}
