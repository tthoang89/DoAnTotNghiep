using AppData.Models;
using AppData.ViewModels;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class QuyDoiDiemController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dbcontext;
        public QuyDoiDiemController()
        {
            _httpClient = new HttpClient();
            dbcontext = new AssignmentDBContext();
        }
        public int PageSize = 10;
        [HttpGet]
        public async Task<IActionResult> GetAllQuyDoiDiem(int ProductPage = 1)
        {
            string apiURL = $"https://localhost:7095/api/QuyDoiDiem";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<QuyDoiDiem>>(apiData);                             
            return View(new PhanTrangQuyDoiDiem
            {
                listqdd= roles
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = roles.Count()
                }

            }
                );

        }
        // tim kiem ten
       
        // create
        public IActionResult Create()
        {
            var Diem = dbcontext.QuyDoiDiems.ToList();
            foreach (var tk in Diem)
            {
                var trangthai = dbcontext.QuyDoiDiems.FirstOrDefault(x => x.ID == tk.ID);
                if (trangthai != null)
                {
                    trangthai.TrangThai = 0;
                    dbcontext.QuyDoiDiems.Update(trangthai);
                }

            }
            dbcontext.SaveChangesAsync();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(QuyDoiDiem qdd)
        {
            if (qdd.TiLeTichDiem ==0||qdd.TiLeTichDiem==null)
            {
                ViewData["TiLeTichDiem"] = "Yêu cầu nhập dữ liệu ";
            }
            else if(qdd.TiLeTichDiem<0)
            {
                ViewData["TiLeTichDiem"] = "Yêu cầu nhập dữ liệu lớn hơn 0 ";
            }
            if (qdd.TiLeTieuDiem ==0||qdd.TiLeTieuDiem==null)
            {
                ViewData["TiLeTieuDiem"] = "Yêu cầu nhập dữ liệu ";
            }
            else if (qdd.TiLeTieuDiem < 0)
            {
                ViewData["TiLeTieuDiem"] = "Yêu cầu nhập dữ liệu lớn hơn 0 ";
            }
            if (qdd.TrangThai == 0)
            {
                ViewData["TrangThai"] = "Yêu cầu chọn trạng thái ";
            }
            else
            {
                var response = await _httpClient.PostAsync($"https://localhost:7095/api/QuyDoiDiem?TiLeTichDiem={qdd.TiLeTichDiem}&TiLeTieuDiem={qdd.TiLeTieuDiem}&TrangThai={qdd.TrangThai}", null);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllQuyDoiDiem");
                }
                return View();
            }
            return View();
                       
        }
        // update
        [HttpGet]
        public IActionResult Updates(Guid id)
        {

            var url = $"https://localhost:7095/api/QuyDoiDiem/{id}";
            var response = _httpClient.GetAsync(url).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            var KhuyenMais = JsonConvert.DeserializeObject<QuyDoiDiem>(result);
            HttpContext.Session.SetString("IDQuyDoi", id.ToString());
            return View(KhuyenMais);
        }

        [HttpPost]

        public async Task<IActionResult> Updates(QuyDoiDiem qdd)
        {
            var Diem = Guid.Parse(HttpContext.Session.GetString("IDQuyDoi"));
            var trangthai = dbcontext.QuyDoiDiems.Where(x => x.ID != Diem).ToList();
            if (trangthai != null)
            {
                foreach (var qd in trangthai)
                {
                    qd.TrangThai = 0;
                    dbcontext.QuyDoiDiems.Update(qd);
                }
                dbcontext.SaveChangesAsync();
                
            }                            
            var response = await _httpClient.PutAsync($"https://localhost:7095/api/QuyDoiDiem/{qdd.ID}?TiLeTichDiem={qdd.TiLeTichDiem}&TiLeTieuDiem={qdd.TiLeTieuDiem}&TrangThai={qdd.TrangThai}", null);
           
            if (response.IsSuccessStatusCode)
            {              
                return RedirectToAction("GetAllQuyDoiDiem");
            }
            return View();                      
        }
        // delete
        public async Task<IActionResult> Delete(Guid id)
        {
            string apiURL = $"https://localhost:7095/api/QuyDoiDiem/{id}";

            var response = await _httpClient.DeleteAsync(apiURL);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllQuyDoiDiem");
            }
            return View();
        }
    }
}
