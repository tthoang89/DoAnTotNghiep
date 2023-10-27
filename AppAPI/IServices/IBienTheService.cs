using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;

namespace AppAPI.IServices
{
    public interface IBienTheService
    {
        #region BienThe
        Task<List<BienTheViewModel>> GetAllBienThe();
        Task<List<BienTheViewModel>> GetBienTheByIdSanPham(Guid idSanPham);
        Task<BienTheViewModel> GetBienTheById(Guid idBienThe);
        Task<BienTheViewModel> GetBTByListGiaTri(BienTheTruyVan bttv);
        Task<BienThe> SaveBienThe(BienTheRequest bt);
        Task<bool> DeleteBienThe(Guid id);
        void SetBienTheDefault(Guid idbt);

        #endregion
        #region Anh
        Task<string> SaveFile(string imagePath); // Lưu file ảnh vào wwwroot trả về tên ảnh
        void SaveAnh(string tenAnh, Guid idbt); //Lưu ảnh
        Task<bool> DeleteAnh(Guid id);
        List<string> GetListImg(List<string> imagePath); // Trả về list đường dẫn ảnh ko trùng lặp
        bool CheckImageExist(string imagePath);

        #endregion
    }
}
