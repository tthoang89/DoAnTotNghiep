using AppData.Models;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface IVaiTroService
    {
        Task<List<VaiTro>> GetAllVaiTro();
        Task<VaiTro> GetVaiTroById(Guid id);
        Task<VaiTro> CreateVaiTro(VaiTro vaiTro);
        Task<bool> DeleteVaiTro(Guid id);
        Task<VaiTro> UpdateVaiTro(Guid id,  VaiTro vaiTro);
    }
}
