using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;

namespace AppAPI.IServices
{
    public interface IBienTheService
    {
        #region BienThe
        Task<List<BienTheViewModel>> GetBienTheByIdSanPham(Guid idSanPham);
        Task<BienTheViewModel> GetBienTheById(Guid idBienThe);
        Task<BienTheViewModel> GetBTByListGiaTri(BienTheTruyVan bttv);
        Task<BienThe> SaveBienThe(BienTheRequest bt);
        Task<bool> DeleteBienThe(Guid id);
        #endregion

        #region Anh
        //Task<List<Anh>> GetAnhByIdBienThe(Guid idBienThe);
        //Task<string> SaveFile(IFormFile file);
        Task<bool> DeleteAnh(Guid id);
        #endregion
    }
}
