using AppAPI.IServices;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class VaiTroSevice : IVaiTroService
    {
        private readonly AssignmentDBContext dBContext;

        public VaiTroSevice()
        {
            this.dBContext = new AssignmentDBContext();
        }

        public async Task<VaiTro> CreateVaiTro(VaiTro vaiTro)
        {
            dBContext.VaiTros.Add(vaiTro);
            await dBContext.SaveChangesAsync();
            return vaiTro;
        }

        public async Task<bool> DeleteVaiTro(Guid id)
        {
            var vt = await dBContext.VaiTros.FindAsync(id);
            if( vt!= null)
            {
                dBContext.VaiTros.Remove(vt);
                await dBContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<List<VaiTro>> GetAllVaiTro()
        {
            return await dBContext.VaiTros.ToListAsync();
        }

        public async Task<VaiTro> GetVaiTroById(Guid id)
        {
            return await dBContext.VaiTros.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<VaiTro> UpdateVaiTro(Guid id, VaiTro vaiTro)
        {
            var vaitro = await dBContext.VaiTros.FindAsync(id);
            if (vaitro== null)
            {
                return null;
            }
            else
            {
                vaitro.Ten = vaiTro.Ten;
                vaitro.TrangThai = vaiTro.TrangThai;
                await dBContext.SaveChangesAsync();
                return vaitro;
            }
        }
    }
}
