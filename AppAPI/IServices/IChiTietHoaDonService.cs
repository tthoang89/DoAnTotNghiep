using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface IChiTietHoaDonService
    {
        public List<ChiTietHoaDon> GetAllCTHoaDon();
        public bool CreateCTHoaDon(ChiTietHoaDon chiTietHoaDon);
        public bool UpdateCTHoaDon(ChiTietHoaDon chiTietHoaDon);
        public bool DeleteCTHoaDon(Guid id);
    }
}
