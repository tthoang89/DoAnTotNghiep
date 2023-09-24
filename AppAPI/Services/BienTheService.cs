using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class BienTheService : IBienTheService
    {
        private readonly AssignmentDBContext _context;
        public BienTheService()
        {
            this._context = new AssignmentDBContext();
        }
        #region BienThe
        public async Task<bool> DeleteBienThe(Guid id)
        {
            var bienthe = await _context.BienThes.FindAsync(id);
            if (bienthe == null) throw new Exception($"Không tìm thấy biến thể có Id: {id}");

            //Xóa Chi tiết biến thể
            var listctbt = await _context.ChiTietBienThes.Where(c => c.IDBienThe == id).ToListAsync();
            listctbt.RemoveRange(0, listctbt.Count);
            await _context.SaveChangesAsync();
            // Xóa list ảnh
            var listAnh = await _context.Anhs.Where(c => c.IDBienThe == id).ToListAsync();
            listAnh.RemoveRange(0, listAnh.Count);
            await _context.SaveChangesAsync();
            //Xóa BT
            _context.BienThes.Remove(bienthe);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<List<Anh>> GetAnhByIdBienThe(Guid idBienThe)
        {
            throw new NotImplementedException();
        }

        public async Task<BienTheViewModel> GetBienTheById(Guid idBienThe)
        {
            if (!_context.BienThes.Any(c => c.ID == idBienThe)) throw new Exception($"Không tìm thấy Sản phẩm có Id: {idBienThe}");
            var bthe = await (from sp in _context.SanPhams
                              join bt in _context.BienThes on sp.ID equals bt.IDSanPham
                              where bt.ID == idBienThe
                              select new BienTheViewModel()
                              {
                                  ID = bt.ID,
                                  Ten = sp.Ten,
                                  SoLuong = bt.SoLuong,
                                  GiaBan = bt.GiaBan,
                                  TrangThai = bt.TrangThai,
                                  Anh = bt.Anh,
                                  GiaTris = (from gt in _context.GiaTris
                                             join ctbt in _context.ChiTietBienThes on gt.ID equals ctbt.IDGiaTri
                                             where ctbt.IDBienThe == bt.ID
                                             select new GiaTriRequest()
                                             {
                                                 ID = gt.ID,
                                                 Ten = gt.Ten
                                             }).ToList()
                              }).FirstOrDefaultAsync();
            return bthe;
        }

        public async Task<List<BienTheViewModel>> GetBienTheByIdSanPham(Guid idProduct)
        {
            if (!_context.SanPhams.Any(c => c.ID == idProduct)) throw new Exception($"Không tìm thấy Sản phẩm có Id: {idProduct}");

            var query = await (from sp in _context.SanPhams
                               join bt in _context.BienThes on sp.ID equals bt.IDSanPham
                               where bt.IDSanPham == idProduct
                               select new BienTheViewModel()
                               {
                                   ID = bt.ID,
                                   SoLuong = bt.SoLuong,
                                   Ten = sp.Ten,
                                   Anh = bt.Anh,
                                   GiaBan = bt.GiaBan,
                                   TrangThai = bt.TrangThai,
                                   GiaTris = (from gt in _context.GiaTris
                                              join ctbt in _context.ChiTietBienThes on gt.ID equals ctbt.IDGiaTri
                                              where ctbt.IDBienThe == bt.ID
                                              select new GiaTriRequest()
                                              {
                                                  ID = gt.ID,
                                                  Ten = gt.Ten
                                              }).ToList()
                               }).ToListAsync();


            return query;
        }        
        public async Task<BienThe> SaveBienThe(BienTheRequest request)
        {
            // Check tồn tại
            var bienthe = await _context.BienThes.FindAsync(request.ID);

            if (bienthe != null) // Update
            {
                bienthe.SoLuong = request.SoLuong;
                bienthe.GiaBan = request.GiaBan;
                bienthe.TrangThai = request.TrangThai;
                bienthe.Anh = request.Anh;
                _context.BienThes.Update(bienthe);
                await _context.SaveChangesAsync();

                // Giá trị
                foreach (var id in request.ListIdGiaTri)
                {
                    // Biến thể chưa có giá trị này -> Tạo CTBT
                    if (!_context.ChiTietBienThes.Where(c => c.IDBienThe == bienthe.ID).Any(c => c.IDGiaTri == id))
                    {
                        //Tạo CTBT
                        ChiTietBienThe ctbt = new ChiTietBienThe()
                        {
                            IDBienThe = bienthe.ID,
                            IDGiaTri = id,
                            TrangThai = 1,
                        };
                        await _context.ChiTietBienThes.AddAsync(ctbt);
                        await _context.SaveChangesAsync();
                    }
                }
                return bienthe;
            }
            else //Tạo mới
            {
                BienThe bienThe = new BienThe()
                {
                    SoLuong = request.SoLuong,
                    NgayTao = DateTime.Now,
                    TrangThai = request.TrangThai,
                    GiaBan = request.GiaBan,
                    IDSanPham = request.IDSanPham,
                    Anh = request.Anh,
                };
                await _context.BienThes.AddAsync(bienThe);
                await _context.SaveChangesAsync();

                // Tạo CTBT
                foreach (var id in request.ListIdGiaTri)
                {
                    ChiTietBienThe ctbt = new ChiTietBienThe()
                    {
                        IDGiaTri = id,
                        IDBienThe = bienThe.ID,
                        TrangThai = 1,
                    };
                    await _context.ChiTietBienThes.AddAsync(ctbt);
                    await _context.SaveChangesAsync();
                }
                return bienThe;
            };
        }
        #endregion
        //#region Anh
        //public Task<bool> SaveAnh(Anh img)
        //{
        //    throw new NotImplementedException();
        //}
        //public async Task<bool> DeleteAnh(Guid id)
        //{
        //    var a = await _context.Anhs.FindAsync(id);
        //    if (a == null)
        //    {
        //        _context.Anhs.Remove(a);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}
        //#endregion
    }

}
