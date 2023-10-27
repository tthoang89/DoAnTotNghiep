using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;

namespace AppAPI.IServices
{
    public interface IChiTietHoaDonService
    {
        public List<ChiTietHoaDon> GetAllCTHoaDon();
        public Task<ChiTietHoaDon> SaveCTHoaDon(HoaDonChiTietRequest chiTietHoaDon);
        public Task<bool> DeleteCTHoaDon(Guid id);
        public Task<List<HoaDonChiTietViewModel>> GetHDCTByIdHD(Guid idhd);

    }
}
