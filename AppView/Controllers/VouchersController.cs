using AppData.Models;
using AppData.ViewModels;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class VouchersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public VouchersController()
        {
            _httpClient = new HttpClient();
            dBContext=new AssignmentDBContext();
        }
        public int PageSize = 8;
        // get all vocher
        [HttpGet]
        public async Task<IActionResult> GetAllVoucher(int ProductPage = 1)
        {
            string apiURL = $"https://localhost:7095/api/Voucher";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<VoucherView>>(apiData);
            return View(new PhanTrangVouchers
            {
                listvouchers = roles
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
        [HttpGet]
        public async Task<IActionResult> TimKiemTenVC(string Ten, int ProductPage = 1)
        {
            string apiURL = $"https://localhost:7095/api/Voucher";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<VoucherView>>(apiData);
            return View("GetAllVoucher", new PhanTrangVouchers
            {
                listvouchers = roles.Where(x => x.Ten.Contains(Ten))
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
        // create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(VoucherView voucher)
        {
            if (voucher.SoTienCan == null || voucher.SoTienCan <= 0)
            {
                ViewData["SoTienCan"] = "Mời bạn nhập số tiền cần";
            }
            if (voucher.GiaTri == null || voucher.GiaTri <= 0)
            {
                ViewData["GiaTri"] = "Mời bạn nhập giá trị ";
            }
            if (voucher.SoLuong == null || voucher.SoLuong <= 0)
            {
                ViewData["SoLuong"] = "Mời bạn nhập số lượng ";
            }
            if (voucher.TrangThai == null || voucher.TrangThai == 0)
            {
                ViewData["TrangThai"] = "Mời bạn chọn trạng thái ";
            }
            if (voucher.NgayApDung == null)
            {
                ViewData["NgayApDung"] = "Mời bạn nhập ngày áp dụng";
            }
            if (voucher.NgayKetThuc == null)
            {
                ViewData["NgayKetThuc"] = "Mời bạn nhập ngày kết thúc";
            }
            if (voucher.NgayKetThuc < voucher.NgayApDung)
            {
                ViewData["Ngay"] = "Ngày kết thúc phải lớn hơn ngày áp dụng";
            }
            else
            {
                var response = await _httpClient.PostAsJsonAsync($"https://localhost:7095/api/Voucher", voucher);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllVoucher");
                }
                return View();
            }

            
            return View();
        }
        // update
        [HttpGet]
        public IActionResult Updates(Guid id)
        {
           
            var url = $"https://localhost:7095/api/Voucher/{id}";
            var response = _httpClient.GetAsync(url).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            var KhuyenMais = JsonConvert.DeserializeObject<VoucherView>(result);
            return View(KhuyenMais);
        }

        [HttpPost]

        public async Task<IActionResult> Updates( VoucherView voucher)
        {
            if (voucher.SoTienCan == null || voucher.SoTienCan == 0)
            {
                ViewData["SoTienCan"] = "Mời bạn nhập số tiền cần";
            }
            if (voucher.GiaTri == null || voucher.GiaTri == 0)
            {
                ViewData["GiaTri"] = "Mời bạn nhập giá trị ";
            }
            if (voucher.SoLuong == null || voucher.SoLuong == 0)
            {
                ViewData["SoLuong"] = "Mời bạn nhập số lượng ";
            }
            if (voucher.TrangThai == null || voucher.TrangThai == 0)
            {
                ViewData["TrangThai"] = "Mời bạn chọn trạng thái ";
            }
            if (voucher.NgayApDung == null)
            {
                ViewData["NgayApDung"] = "Mời bạn nhập ngày áp dụng";
            }
            if (voucher.NgayKetThuc == null)
            {
                ViewData["NgayKetThuc"] = "Mời bạn nhập ngày kết thúc";
            }
            if (voucher.NgayKetThuc < voucher.NgayApDung)
            {
                ViewData["Ngay"] = "Ngày kết thúc phải lớn hơn ngày áp dụng";
            }
            else
            {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/Voucher/{voucher.Id}", voucher);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllVoucher");
                }
                return View();
            }
           
            return View();
        }
        // delete
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    string apiURL = $"https://localhost:7095/api/Voucher/{id}";

        //    var response = await _httpClient.DeleteAsync(apiURL);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("GetAllVoucher");
        //    }
        //    return View();
        //}
        
        public async Task<IActionResult> SuDung(Guid id)
        {
            var timkiem = dBContext.Vouchers.FirstOrDefault(x => x.ID == id);
            if (timkiem != null)
            {
                timkiem.TrangThai = 1;
                dBContext.Vouchers.Update(timkiem);
                dBContext.SaveChanges();
                return RedirectToAction("GetAllVoucher");
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> KoSuDung(Guid id)
        {
            var timkiem = dBContext.Vouchers.FirstOrDefault(x => x.ID == id);
            if (timkiem != null)
            {
                timkiem.TrangThai = 0;
                dBContext.Vouchers.Update(timkiem);
                dBContext.SaveChanges();
                return RedirectToAction("GetAllVoucher");
            }
            else
            {
                return View();
            }
        }
    }
}
