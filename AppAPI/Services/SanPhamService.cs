using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AssignmentDBContext _context;
        public SanPhamService()
        {
            this._context = new AssignmentDBContext();
        }
        #region ThuocTinh
        public async Task<List<ThuocTinhRequest>> GetAllThuocTinh()
        {
            var listtt = await (from tt in _context.ThuocTinhs
                                select new ThuocTinhRequest()
                                {
                                    ID = tt.ID,
                                    TrangThai = tt.TrangThai,
                                    Ten = tt.Ten,
                                    GiaTriRequests = (from gt in _context.GiaTris
                                                      where tt.ID == gt.IdThuocTinh
                                                      select new GiaTriRequest()
                                                      {
                                                          ID = gt.ID,
                                                          Ten = gt.Ten,
                                                      }).ToList()
                                }).ToListAsync();
            return listtt;
        }
        public async Task<ThuocTinh> SaveThuocTinh(ThuocTinhRequest tt)
        {
            var exist = await _context.ThuocTinhs.FindAsync(tt.ID);
            if (exist != null)//Tồn tại
            {
                // Check trùng tên
                if (_context.ThuocTinhs.Where(c => c.ID != exist.ID).Any(c => c.Ten == tt.Ten))
                    throw new Exception($"Thuộc tính {tt.Ten} đã tồn tại");

                exist.Ten = tt.Ten;
                exist.TrangThai = tt.TrangThai;
                _context.ThuocTinhs.Update(exist);
                await _context.SaveChangesAsync();

                // Giá trị
                foreach (var gt in tt.GiaTriRequests)
                {
                    // Check TT đã có GT này
                    var gtr = await _context.GiaTris.Where(c => c.IdThuocTinh == exist.ID && c.ID == gt.ID).FirstOrDefaultAsync();

                    if (gtr == null) // Chưa có -> Tạo GT
                    {
                        // Check trùng tên
                        if (_context.GiaTris.Any(c => c.Ten == gt.Ten))
                            throw new Exception("Thuộc tính đã tồn tại Giá trị này");

                        GiaTri giaTri = new GiaTri()
                        {
                            IdThuocTinh = exist.ID,
                            Ten = gt.Ten,
                        };
                        await _context.GiaTris.AddAsync(giaTri);
                        await _context.SaveChangesAsync();
                    }
                }
                return exist;
            }
            else // Chưa có -> Tạo Thuộc tính ms
            {
                // Check trùng tên
                if (_context.ThuocTinhs.Any(c => c.Ten == tt.Ten))
                    throw new Exception($"Thuộc tính {tt.Ten} đã tồn tại");

                ThuocTinh thuocTinh = new ThuocTinh()
                {
                    Ten = tt.Ten,
                    NgayTao = DateTime.Now,
                    TrangThai = tt.TrangThai,
                };
                await _context.ThuocTinhs.AddAsync(thuocTinh);
                await _context.SaveChangesAsync();
                // Tạo GT
                foreach (var gt in tt.GiaTriRequests)
                {
                    GiaTri giaTri = new GiaTri()
                    {
                        IdThuocTinh = thuocTinh.ID,
                        Ten = gt.Ten,
                    };
                    await _context.GiaTris.AddAsync(giaTri);
                    await _context.SaveChangesAsync();
                }
                return thuocTinh;
            }

        }
        public async Task<bool> DeleteThuocTinh(Guid id)
        {
            // Kiểm tra thuộc tính đã có sản phẩm nào sử dụng
            var tt = await _context.ThuocTinhs.FindAsync(id);
            if (tt != null)
            {
                // Xóa toàn bộ giá trị 
                var listgtr = await _context.GiaTris.Where(c => c.IdThuocTinh == tt.ID).ToListAsync();
                listgtr.RemoveRange(0, listgtr.Count);
                // Xóa thuộc tính
                _context.Remove(tt);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region SanPham
        //TÌM KIẾM NÂNG CAO : Tên, List Loại Sp, khoảng Giá
        public async Task<List<SanPham>> TimKiemSanPham(SanPhamTimKiemNangCao sptk)
        {
            // Lấy ds Sản phẩm cùng loại sp của chúng
            var sanphams = (from sp in _context.SanPhams
                            join lsp in _context.LoaiSPs on sp.IDLoaiSP equals lsp.ID
                            join bt in _context.BienThes on sp.ID equals bt.IDSanPham
                            select new { sp, lsp, bt });
            // Tìm tên
            if (!string.IsNullOrEmpty(sptk.KeyWord))
            {
                sanphams = sanphams.Where(c => c.sp.Ten.ToLower().Contains(sptk.KeyWord.ToLower()));
            }
            // Loại sp
            if (sptk.IdLoaiSP.Count() > 0)
            {
                sanphams = sanphams.Where(c => sptk.IdLoaiSP.Contains(c.lsp.ID));
            }
            // Khoảng giá
            if (sptk.GiaMax != 0 && sptk.GiaMin != 0)
            {
                sanphams = sanphams.Where(c => c.bt.GiaBan >= sptk.GiaMin && c.bt.GiaBan <= sptk.GiaMax);
            }
            var sP = await (from sps in sanphams
                            join sp in _context.SanPhams on sps.sp.ID equals sp.ID
                            select new SanPham()
                            {
                                ID = sp.ID,
                                Ten = sp.Ten,
                                MoTa = sp.MoTa,
                                TrangThai = sp.TrangThai,
                                IDLoaiSP = sp.IDLoaiSP
                            }).ToListAsync();
            return sP;
        }

        //CHECK TRÙNG TÊN SP
        public bool CheckTrungTenSP(SanPhamRequest lsp)
        {
            if (_context.SanPhams.Any(c => c.Ten == lsp.Ten && c.ID != lsp.ID))
            {
                return false;
            }
            return true;
        }
        // XÓA SẢN PHẨM
        public async Task<bool> DeleteSanPham(Guid id)
        {
            var sp = await _context.SanPhams.FindAsync(id);
            // Xóa TT-SP
            var listTTSP = await _context.ThuocTinhSanPhams.Where(c => c.IDSanPham == id).ToListAsync();
            listTTSP.RemoveRange(0, listTTSP.Count);
            await _context.SaveChangesAsync();

            // Xóa Biến thể : Xóa CTBT, Ảnh -> Xóa BT
            var listBT = await _context.BienThes.Where(c => c.IDSanPham == id).ToListAsync();
            foreach (var bt in listBT)
            {
                //Xóa CTBT
                var listCTBT = await _context.ChiTietBienThes.Where(c => c.IDBienThe == bt.ID).ToListAsync();
                listCTBT.RemoveRange(0, listCTBT.Count);
                await _context.SaveChangesAsync();
                //Xóa Ảnh
                var listAnh = await _context.Anhs.Where(c => c.IDBienThe == bt.ID).ToListAsync();
                listAnh.RemoveRange(0, listAnh.Count);
                await _context.SaveChangesAsync();
            }
            listBT.RemoveRange(0, listBT.Count);
            await _context.SaveChangesAsync();

            _context.SanPhams.Remove(sp);
            await _context.SaveChangesAsync();
            return true;
        }
        // LẤY DS SẢN PHẨM
        public async Task<List<SanPhamViewModel>> GetAllSanPham()
        {
            var sanphams = await (from sp in _context.SanPhams
                                  join lsp in _context.LoaiSPs on sp.IDLoaiSP equals lsp.ID
                                  select new SanPhamViewModel()
                                  {
                                      ID = sp.ID,
                                      Ten = sp.Ten,
                                      MoTa = sp.MoTa,
                                      TrangThai = sp.TrangThai,
                                      LoaiSP = lsp.Ten,
                                      Image = _context.BienThes.Where(c => c.IDSanPham == sp.ID).FirstOrDefault().Anh,
                                      ThuocTinhs = (from tt in _context.ThuocTinhs 
                                                   join ttsp in _context.ThuocTinhSanPhams
                                                   on tt.ID equals ttsp.IDThuocTinh
                                                   where ttsp.IDSanPham == sp.ID
                                                   select new ThuocTinh()
                                                   {
                                                       ID = tt.ID,
                                                       Ten = tt.Ten
                                                   }).ToList(),
                                                    
                                  }).ToListAsync();
            return sanphams;
        }
        // Hiển thị Sản phẩm và Biến thể qua Id
        public async Task<SanPhamViewModel> GetSanPhamById(Guid id)
        {
            var sanpham = await (from sp in _context.SanPhams
                                  join lsp in _context.LoaiSPs on sp.IDLoaiSP equals lsp.ID
                                  where sp.ID == id
                                  select new SanPhamViewModel()
                                  {
                                      ID = sp.ID,
                                      Ten = sp.Ten,
                                      MoTa = sp.MoTa,
                                      TrangThai = sp.TrangThai,
                                      LoaiSP = lsp.Ten,
                                      Image = _context.BienThes.Where(c => c.IDSanPham == sp.ID).FirstOrDefault().Anh,
                                      ThuocTinhs = (from tt in _context.ThuocTinhs
                                                    join ttsp in _context.ThuocTinhSanPhams
                                                    on tt.ID equals ttsp.IDThuocTinh
                                                    where ttsp.IDSanPham == sp.ID
                                                    select new ThuocTinh()
                                                    {
                                                        ID = tt.ID,
                                                        Ten = tt.Ten
                                                    }).ToList(),
                                      BienThes = (from bt in _context.BienThes
                                                  where bt.IDSanPham == sp.ID
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
                                                  }).ToList()
                                  }).FirstOrDefaultAsync();
            return sanpham;
        }
        // TẠO HOẶC CẬP NHẬT SẢN PHẨM
        public async Task<SanPham> SaveSanPham(SanPhamRequest request)
        {
            //Kiểm tra tồn tại
            var sanpham = await _context.SanPhams.FindAsync(request.ID);
            if (sanpham != null)//Update
            {
                sanpham.MoTa = request.MoTa;
                sanpham.Ten = request.Ten;
                sanpham.IDLoaiSP = request.IDLoaiSP;
                sanpham.TrangThai = request.TrangThai;
                _context.SanPhams.Update(sanpham);
                await _context.SaveChangesAsync();

                //Thuộc tính
                foreach (var id in request.ListIdThuocTinh)
                {
                    var thuoctinh = await _context.ThuocTinhs.FindAsync(id);

                    // Check SP đã có TT này chưa
                    var ttspExist = await _context.ThuocTinhSanPhams.FirstOrDefaultAsync(c => c.IDSanPham == sanpham.ID && c.IDThuocTinh == thuoctinh.ID);

                    if (ttspExist == null) // Chưa có -> tạo SP-TT
                    {
                        ThuocTinhSanPham ttsp = new ThuocTinhSanPham()
                        {
                            IDSanPham = sanpham.ID,
                            IDThuocTinh = id,
                        };
                        await _context.ThuocTinhSanPhams.AddAsync(ttsp);
                        await _context.SaveChangesAsync();
                    }
                }
                return sanpham;
            }
            else // Ngược lại ->Tạo mới
            {
                SanPham sp = new SanPham()
                {
                    Ten = request.Ten,
                    MoTa = request.MoTa,
                    TrangThai = request.TrangThai,
                    IDLoaiSP = request.IDLoaiSP,
                };
                await _context.SanPhams.AddAsync(sp);
                await _context.SaveChangesAsync();

                foreach (var id in request.ListIdThuocTinh)
                {
                    var thuoctinh = await _context.ThuocTinhs.FindAsync(id);
                    ThuocTinhSanPham ttsp = new ThuocTinhSanPham()
                    {
                        IDSanPham = sp.ID,
                        IDThuocTinh = id,
                    };
                    await _context.ThuocTinhSanPhams.AddAsync(ttsp);
                    await _context.SaveChangesAsync();
                }
                return sp;
            }
        }
        #endregion

    }
}
