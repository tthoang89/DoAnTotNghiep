using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface IBienTheService
    {
        #region BienThe
        Task<List<BienTheViewModel>> GetBienTheByIdSanPham(Guid idSanPham);
        Task<BienTheViewModel> GetBienTheById(Guid idBienThe);
        Task<BienThe> SaveBienThe(BienTheRequest bt);
        Task<bool> DeleteBienThe(Guid id);

        #endregion
        //#region Anh
        //Task<List<Anh>> GetAnhByIdBienThe(Guid idBienThe);
        //Task<bool> SaveAnh(Anh img);
        //Task<bool> DeleteAnh(Guid id);
        //#endregion
    }
}
