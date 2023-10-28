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

        #region ThuocTinh
        Task<List<ThuocTinhRequest>> GetAllThuocTinh();
        Task<ThuocTinhRequest> GetByID(Guid idtt);
        Task<ThuocTinh> SaveThuocTinh(ThuocTinhRequest tt);
        Task<bool> DeleteThuocTinh(Guid id);
        Task<bool> CheckTrungTT(ThuocTinhRequest tt);
        //Tam
        Task<List<GiaTri>> GetGiaTri(string thuocTinh);
        //Task<bool> AddSanPham(SanPhamRequestMVC sanPham);
        //End
        #endregion       
    }
}
