using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface ISanPhamService
    {
        #region SanPham
        Task<List<SanPhamViewModel>> GetAllSanPham();
        Task<List<SanPhamViewModel>> TimKiemSanPham(SanPhamTimKiemNangCao sp);
        Task<SanPhamDetail> GetSanPhamById(Guid id);
        Task<List<SanPhamViewModel>> GetSanPhamByIdDanhMuc(Guid idloaisp);
        Task<SanPham> SaveSanPham(SanPhamRequest request);
        Task<bool> DeleteSanPham(Guid id);
        bool CheckTrungTenSP(SanPhamRequest lsp);
        #endregion

        #region LoaiSanPham
        Task<List<LoaiSP>> GetAllLoaiSP();
        Task<LoaiSP> GetLoaiSPById(Guid id);
        Task<LoaiSP> SaveLoaiSP(LoaiSPRequest lsp);
        Task<bool> DeleteLoaiSP(Guid id);
        bool CheckTrungLoaiSP(LoaiSPRequest lsp);
        Task<List<LoaiSP>> GetLoaiSPCha();
        Task<List<LoaiSP>> GetLoaiSPCon(Guid idLoaiSPCha);
        #endregion

        #region ChiTietSanPham
        Task<List<ChiTietSanPham>> GetAllChiTietSanPham(Guid idSanPham);
        Task<bool> DeleteChiTietSanPham(Guid id);
        Task<bool> UpdateChiTietSanPham(ChiTietSanPham chiTietSanPham);
        Task<bool> AddChiTietSanPham(ChiTietSanPham chiTietSanPham);
        #endregion
    }
}
