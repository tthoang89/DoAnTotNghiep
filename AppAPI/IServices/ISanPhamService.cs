using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface ISanPhamService
    {
        #region SanPham
        Task<List<SanPhamViewModel>> GetAllSanPham();
        Task<List<SanPham>> TimKiemSanPham(SanPhamTimKiemNangCao sp);
        Task<SanPhamViewModel> GetSanPhamById(Guid id);
        Task<SanPham> SaveSanPham(SanPhamRequest request);
        Task<bool> DeleteSanPham(Guid id);
        bool CheckTrungTenSP(SanPhamRequest lsp);
        #endregion

        #region ThuocTinh
        Task<List<ThuocTinhRequest>> GetAllThuocTinh();
        Task<ThuocTinh> SaveThuocTinh(ThuocTinhRequest tt);
        Task<bool> DeleteThuocTinh(Guid id);
        #endregion       
    }
}
