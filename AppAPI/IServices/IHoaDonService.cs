using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;

namespace AppAPI.IServices
{
    public interface IHoaDonService
    {
        public bool CreateHoaDon(List<ChiTietHoaDonViewModel> chiTietHoaDons,HoaDonViewModel hoaDon);
        public bool CreateHoaDonOffline(Guid idnhanvien);
        public List<HoaDon> GetAllHoaDon();
        public HoaDon GetHoaDonById(Guid idhd);
        public List<ChiTietHoaDon> GetAllChiTietHoaDon(Guid idHoaDon);
        public bool UpdateTrangThaiGiaoHang(Guid idHoaDon, int trangThai,Guid idNhanVien);
        public int CheckVoucher(string ten, int tongtien);
        public List<HoaDon> TimKiemVaLocHoaDon(string ten,int? loc);
        public List<HoaDon> LichSuGiaoDich(Guid idNguoiDung);
        public bool CheckHDHasLSGD( Guid idHoaDon);
        public LichSuTichDiem GetLichSuGiaoDichByIdHD(Guid idHoaDon);
        public bool DeleteHoaDon(Guid id);
        public bool UpdateHoaDon(HoaDonThanhToanRequest hoaDon);
        public List<HoaDon> GetAllHDCho();
        //Phương thức thanh toán
        public List<PhuongThucThanhToan> GetAllPTTT();
        public bool CreatePTTT(PhuongThucThanhToan pttt);
        public bool UpdatePTTT(PhuongThucThanhToan pttt);   
        public bool DeletePTTT(Guid id);   
    }
}
