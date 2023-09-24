using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface IHoaDonService
    {
        public bool CreateHoaDon(List<ChiTietHoaDonViewModel> chiTietHoaDons,HoaDonViewModel hoaDon);
        public List<HoaDon> GetAllHoaDon();
        public List<ChiTietHoaDon> GetAllChiTietHoaDon(Guid idHoaDon);
        public bool UpdateTrangThaiGiaoHang(Guid idHoaDon, int trangThai);
        
    }
}
