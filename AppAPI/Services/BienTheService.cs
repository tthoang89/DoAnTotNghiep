using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
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
            //Xóa Chi tiết biến thể
            var listctbt = await _context.ChiTietBienThes.Where(c => c.IDBienThe == id).ToListAsync();
            _context.ChiTietBienThes.RemoveRange(listctbt);
            //Xóa BT
            _context.BienThes.Remove(bienthe);
            await _context.SaveChangesAsync();
            return true;
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
        // Truy vấn BT thông qua list Giá trị
        public async Task<BienTheViewModel> GetBTByListGiaTri(BienTheTruyVan bttv)
        {
            var query = await (from ctbt in _context.ChiTietBienThes
                               join bt in _context.BienThes on ctbt.IDBienThe equals bt.ID
                               join sp in _context.SanPhams on bt.IDSanPham equals sp.ID
                               where sp.ID == bttv.idSp
                               select ctbt).ToListAsync();

            var results = from p in query
                          group p.IDGiaTri by p.IDBienThe into g
                          select new { IdBienThe = g.Key, GiaTris = g.ToList() };
            Guid idbt = Guid.Empty;
            foreach (var item in results)
            {
                var areEquivalent = (item.GiaTris.Count() == bttv.lstIdGTri.Count()) && !item.GiaTris.Except(bttv.lstIdGTri).Any();
                if (areEquivalent == true)
                {
                    idbt = item.IdBienThe;
                    break;
                }

            }
            var bthe = await GetBienTheById(idbt);
            return bthe;
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
                //Xóa CTBT không còn SD
                var lstIdCTBT = await _context.ChiTietBienThes.Where(c => c.IDBienThe == bienthe.ID).OrderByDescending(c => c.NgayLuu).Select(c => c.ID).Take(request.ListIdGiaTri.Count).ToListAsync();
                var lstDelete = await _context.ChiTietBienThes.Where(c => !lstIdCTBT.Contains(c.ID) & c.IDBienThe == bienthe.ID).ToListAsync();
                _context.ChiTietBienThes.RemoveRange(lstDelete);
                await _context.SaveChangesAsync();
                return bienthe;
            }
            else //Tạo mới
            {
                var sp = await _context.SanPhams.FindAsync(request.IDSanPham);
                if (sp.TrangThai == 0) { request.TrangThai = 0; }
                else { request.TrangThai = 1; };
                BienThe bienThe = new BienThe()
                {
                    ID = new Guid(),
                    SoLuong = request.SoLuong,
                    NgayTao = DateTime.Now,
                    TrangThai = request.TrangThai,
                    GiaBan = request.GiaBan,
                    IDSanPham = request.IDSanPham,
                };
                await _context.BienThes.AddAsync(bienThe);
                await _context.SaveChangesAsync();

                // Tạo CTBT
                foreach (var id in request.ListIdGiaTri)
                {
                    ChiTietBienThe ctbt = new ChiTietBienThe()
                    {
                        ID = new Guid(),
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
        #region Anh

        public async Task<bool> DeleteAnh(Guid id)
        {
            var a = await _context.Anhs.FindAsync(id);
            if (a == null)
            {
                _context.Anhs.Remove(a);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
    }

}
