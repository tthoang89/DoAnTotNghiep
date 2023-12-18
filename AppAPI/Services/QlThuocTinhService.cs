using AppAPI.IServices;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class QlThuocTinhService : IQlThuocTinhService
    {
        private readonly
        AssignmentDBContext _dbContext;
        public QlThuocTinhService()
        {
            _dbContext = new AssignmentDBContext();
        }
        #region KichCo
        public async Task<KichCo> AddKichCo(string ten, int trangthai)
        {
            var existingColor = await _dbContext.KichCos.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper());
            if (existingColor != null)
            {
                return null;
            }
            KichCo kc = new KichCo()
            {
                ID = Guid.NewGuid(),
                Ten = ten,
                TrangThai = 1
            };
            _dbContext.KichCos.Add(kc);
            _dbContext.SaveChanges();
            return kc;
        }
        public async Task<bool> DeleteKichCo(Guid id)
        {
            var nv = await _dbContext.KichCos.FirstOrDefaultAsync(nv => nv.ID == id);
            if (nv != null)
            {
                _dbContext.KichCos.Remove(nv);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<KichCo> UpdateKichCo(Guid id, string ten, int trangthai)
        {
            var nv = await _dbContext.KichCos.FirstOrDefaultAsync(x => x.ID == id);
            if (nv != null)
            {
                var existingColor = await _dbContext.KichCos.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper());
                if (existingColor != null)
                {
                    return null; // Trả về null để báo hiệu tên trùng
                }
                nv.Ten = ten;
                nv.TrangThai = 1;
                _dbContext.KichCos.Update(nv);
                _dbContext.SaveChanges();
                return nv;
            }

            return null;
        }
        public async Task<List<KichCo>> GetAllKichCo()
        {
            return await _dbContext.KichCos.OrderByDescending(x => x.TrangThai).ToListAsync();
        }
        public async Task<KichCo> GetKickCoById(Guid id)
        {
            var nv = await _dbContext.KichCos.FirstOrDefaultAsync(nv => nv.ID == id);
            return nv;
        }

        #endregion MauSac


        public async Task<MauSac> AddMauSac(string ten, string ma, int trangthai)
        {
            var existingColor = await _dbContext.MauSacs.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper() && x.Ma.Trim().ToUpper()==ma.Trim().ToUpper());
            if (existingColor != null)
            {
                return null;
            }

            MauSac kc = new MauSac()
            {
                ID = Guid.NewGuid(),
                Ten = ten,
                Ma = ma,
                TrangThai = 1
            };
            _dbContext.MauSacs.Add(kc);
            _dbContext.SaveChanges();
            return kc;
        }
        public async Task<bool> DeleteMauSac(Guid id)
        {
            var nv = await _dbContext.MauSacs.FirstOrDefaultAsync(nv => nv.ID == id);
            if (nv != null)
            {
                _dbContext.MauSacs.Remove(nv);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<MauSac>> GetAllMauSac()
        {
            return await _dbContext.MauSacs.OrderByDescending(x => x.TrangThai).ToListAsync();
        }

        public async Task<MauSac> GetMauSacById(Guid id)
        {
            var nv = await _dbContext.MauSacs.FirstOrDefaultAsync(nv => nv.ID == id);
            return nv;
        }

        public async Task<MauSac> UpdateMauSac(Guid id, string ten, string ma, int trangthai)
        {
            var nv = await _dbContext.MauSacs.FirstOrDefaultAsync(x => x.ID == id);
            if (nv != null)
            {
                var existingColor = await _dbContext.MauSacs.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper() && x.Ma.Trim().ToUpper() == ma.Trim().ToUpper());
                if (existingColor == null)
                {
                    return null; // Trả về null để báo hiệu tên trùng
                }
                nv.Ten = ten;
                nv.Ma = ma;
                nv.TrangThai = 1;
                _dbContext.MauSacs.Update(nv);
                _dbContext.SaveChanges();
                return nv;
            }

            return null;
        }
        #region chat lieu
        public async Task<ChatLieu> AddChatLieu(string ten, int trangthai)
        {
            var existingColor = await _dbContext.ChatLieus.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper());
            if (existingColor != null)
            {
                return null;
            }
            ChatLieu kc = new ChatLieu()
            {
                ID = Guid.NewGuid(),
                Ten = ten,
                TrangThai = 1
            };
            _dbContext.ChatLieus.Add(kc);
            _dbContext.SaveChanges();
            return kc;
        }

        public async Task<ChatLieu> GetChatLieuById(Guid id)
        {
            var nv = await _dbContext.ChatLieus.FirstOrDefaultAsync(nv => nv.ID == id);
            return nv;
        }

        public async Task<bool> DeleteChatLieu(Guid id)
        {

            var nv = await _dbContext.ChatLieus.FirstOrDefaultAsync(nv => nv.ID == id);
            if (nv != null)
            {
                _dbContext.ChatLieus.Remove(nv);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<ChatLieu> UpdateChatLieu(Guid id, string ten, int trangthai)
        {

            var nv = await _dbContext.ChatLieus.FirstOrDefaultAsync(x => x.ID == id);
            if (nv != null)
            {
                var existingColor = await _dbContext.ChatLieus.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper());
                if (existingColor != null)
                {
                    return null; // Trả về null để báo hiệu tên trùng
                }
                nv.Ten = ten;
                nv.TrangThai = 1;
                _dbContext.ChatLieus.Update(nv);
                _dbContext.SaveChanges();
                return nv;
            }

            return null;
        }

        public async Task<List<ChatLieu>> GetAllChatLieu()
        {
            return await _dbContext.ChatLieus.OrderByDescending(x => x.TrangThai).ToListAsync();
        }
        #endregion
    }
}
