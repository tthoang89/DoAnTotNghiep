using AppData.ViewModels;
using AppData.ViewModels.ThongKe;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly HttpClient _httpClient;
        public ThongKeController()
        {
            _httpClient = new HttpClient();
        }
        #region Thống Kê Sản Phẩm Được Mua nhiều Theo Ngày, Tháng, Năm 
        [HttpGet]
        
        public async Task<IActionResult> ThongKeSP()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeMSSanPhamBan";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeMSSanPhamTheoSoLuong>>(apiData);

            return View(ChatLieus);
        }
        [HttpGet]
        public async Task<IActionResult> ThongKeSPTheoThang(DateTime? ngay, DateTime? thang, DateTime? nam)
        {

            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeMSSanPhamBan";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeMSSanPhamTheoSoLuong>>(apiData);
            var timkiem = ChatLieus.ToList();

            if (ngay.HasValue)
            {
                timkiem = ChatLieus.Where(x => x.Ngay.Date == ngay.Value.Date && x.Ngay.Month == ngay.Value.Month && x.Ngay.Year == ngay.Value.Year).ToList();
            }
            if (thang.HasValue)
            {
                timkiem = ChatLieus.Where(x => x.Ngay.Month == thang.Value.Month && x.Ngay.Year == thang.Value.Year).ToList();
            }
            if (nam.HasValue)
            {
                timkiem = ChatLieus.Where(x => x.Ngay.Year == nam.Value.Year).ToList();
            }

            return View("ThongKeSP", timkiem);
        }
        #endregion
        #region Thông Kê Khách Hàng Mua Nhiêù Theo Ngày, Tháng , Năm
        public int PageSize = 8;
        [HttpGet]

        public async Task<IActionResult> ThongKeKH(int ProductPage = 1)
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeKHTheoSoLuongHoaDon";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeKHMuaNhieu>>(apiData);

            return View(new PhanTrangThongKeKH
            {
                listkhs = ChatLieus
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = ChatLieus.Count()
                }

            }
                );
        }
        [HttpGet]
        public async Task<IActionResult> ThongKeSPKHTheoThang(DateTime ThangStart, DateTime ThangEnd, int ProductPage = 1)
        {

            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeKHTheoSoLuongHoaDon";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeKHMuaNhieu>>(apiData);
            var timkiem = ChatLieus.ToList();

            timkiem = timkiem.Where(x => x.Ngay.Month >= ThangStart.Month && x.Ngay.Month <= ThangEnd.Month).ToList();

            return View("ThongKeKH",new PhanTrangThongKeKH
            {
                listkhs = timkiem
                         .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = timkiem.Count()
                }

            }
                 );
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLSTDByIDKH(Guid id,int ProductPage = 1)
        {
            //
            string apiURL = $"https://localhost:7095/api/LichSuTichDiem/GetLSTDByIdKH?idkh={id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<LichSuTichDiemView>>(apiData);
            return View(new PhanTrangLSTD
            {
                listLSTDs = roles
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
        #endregion
        #region Thông Kê Doanh Thu  Theo Ngày, Tháng , Năm
        [HttpGet]
        public async Task<IActionResult> ThongKeDoanhThuTheoNgay()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNgay";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);

            return View(ChatLieus);
        }
        [HttpGet]
        public async Task<IActionResult> LocThongKeDoanhThuTheoNgay(DateTime NgayStart,DateTime NgayEnd)
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNgay";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);
            var timkiem = ChatLieus.ToList();
            timkiem=timkiem.Where(x=>x.Ngay.Date>=NgayStart.Date&&x.Ngay.Date<=NgayEnd.Date).ToList();
            return View("ThongKeDoanhThuTheoNgay", timkiem);
        }
        [HttpGet]
        public async Task<IActionResult> ThongKeDoanhThuTheoThang()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoThang";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);

            return View(ChatLieus);
        }
        [HttpGet]
        public async Task<IActionResult> LocThongKeDoanhThuTheoThang(DateTime NgayStart, DateTime NgayEnd)
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoThang";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);
            var timkiem = ChatLieus.ToList();
            timkiem = timkiem.Where(x => x.Ngay.Month >= NgayStart.Month && x.Ngay.Month<= NgayEnd.Month).ToList();

            return View("ThongKeDoanhThuTheoThang", timkiem);
        }
        [HttpGet]
        public async Task<IActionResult> ThongKeDoanhThuTheoNam()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNam";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);

            return View(ChatLieus);
        }
        [HttpGet]
        public async Task<IActionResult> LocThongKeDoanhThuTheoNam(DateTime NgayStart, DateTime NgayEnd)
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNam";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);
            var timkiem = ChatLieus.ToList();
            timkiem = timkiem.Where(x => x.Ngay.Year >= NgayStart.Year && x.Ngay.Year <= NgayEnd.Year).ToList();
            return View("ThongKeDoanhThuTheoNam", timkiem);
        }
        #endregion
    }
}
