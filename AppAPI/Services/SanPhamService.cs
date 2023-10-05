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
                        GiaTri giaTri = new GiaTri()
                        {
                            ID = new Guid(),
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
                        ID = new Guid(),
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
                await _context.SaveChangesAsync();
                // Xóa thuộc tính
                _context.Remove(tt);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> CheckTrungTT(ThuocTinhRequest tt)
        {
            if (!_context.ThuocTinhs.Any(c => c.Ten == tt.Ten && c.ID != tt.ID))
            {
                return true;
            }
            return false;
        }

        public async Task<ThuocTinhRequest> GetByID(Guid idtt)
        {
            var result = await (from tt in _context.ThuocTinhs
                                where tt.ID == idtt
                                select new ThuocTinhRequest()
                                {
                                    ID = tt.ID,
                                    Ten = tt.Ten,
                                    TrangThai = tt.TrangThai,
                                    GiaTriRequests = (from gt in _context.GiaTris
                                                      where gt.IdThuocTinh == tt.ID
                                                      select new GiaTriRequest()
                                                      {
                                                          ID = gt.ID,
                                                          Ten = gt.Ten,
                                                      }).ToList()

                                }).FirstOrDefaultAsync();
            return result;
        }
        #endregion



        #region SanPham
        //TÌM KIẾM NÂNG CAO : Tên, List Loại Sp, khoảng Giá
        public async Task<List<SanPhamViewModel>> TimKiemSanPham(SanPhamTimKiemNangCao sptk)
        {
            // Lấy ds Sản phẩm cùng loại sp của chúng
            var sanphams = (from sp in _context.SanPhams
                            join bt in _context.BienThes on sp.ID equals bt.IDSanPham
                            join lsp in _context.LoaiSPs on sp.IDLoaiSP equals lsp.ID
                            select new { sp, bt });
            // Tìm tên
            if (!string.IsNullOrEmpty(sptk.KeyWord))
            {
                sanphams = sanphams.Where(c => c.sp.Ten.ToLower().Contains(sptk.KeyWord.ToLower()));
            }
            // Loại sp
            if (sptk.IdLoaiSP.Count() > 0)
            {
                sanphams = sanphams.Where(c => sptk.IdLoaiSP.Contains(c.sp.IDLoaiSP));
            }
            // Khoảng giá
            if (sptk.GiaMax != 0 && sptk.GiaMin != 0)
            {
                sanphams = sanphams.Where(c => c.bt.GiaBan >= sptk.GiaMin && c.bt.GiaBan <= sptk.GiaMax);
            }
            var result = await sanphams.Select(c => new SanPhamViewModel()
            {
                ID = c.sp.ID,
                Ten = c.sp.Ten,
                MoTa = c.sp.MoTa,
                TrangThai = c.sp.TrangThai
            }).ToListAsync();
            return result;
        }
        //CHECK TRÙNG TÊN SP
        public bool CheckTrungTenSP(SanPhamRequest lsp)
        {
            if (!_context.SanPhams.Any(c => c.Ten == lsp.Ten && c.ID != lsp.ID))
            {
                return true;
            }
            return false;
        }
        // LẤY DS SẢN PHẨM
        public async Task<List<SanPhamViewModel>> GetAllSanPham()
        {
            var sanphams = await (from sp in _context.SanPhams
                                  join lsp in _context.LoaiSPs on sp.IDLoaiSP equals lsp.ID
                                  join bt in _context.BienThes on sp.ID equals bt.IDSanPham
                                  where bt.TrangThai == 1 // Sản phẩm hoạt động
                                  select new SanPhamViewModel()
                                  {
                                      ID = sp.ID,
                                      Ten = sp.Ten,
                                      MoTa = sp.MoTa,
                                      TrangThai = sp.TrangThai,
                                      LoaiSP = lsp.Ten,
                                      GiaGoc = bt.GiaBan,
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
        // Lấy DS sản phẩm theo IdLsp
        public async Task<List<SanPhamViewModel>> GetSanPhamByIdDanhMuc(Guid idloaisp)
        {
            var lsp = await _context.LoaiSPs.FindAsync(idloaisp);
            if (lsp.IDLoaiSPCha == null)
            {
                var lspc = await _context.LoaiSPs.Where(c => c.IDLoaiSPCha == lsp.ID).ToListAsync();
                var result = await (from sp in _context.SanPhams
                                    join ct in lspc on sp.IDLoaiSP equals ct.ID
                                    join bt in _context.BienThes on sp.ID equals bt.IDSanPham
                                    select new SanPhamViewModel()
                                    {
                                        ID = sp.ID,
                                        Ten = sp.Ten,
                                        TrangThai = sp.TrangThai,
                                        LoaiSP = ct.Ten,
                                        MoTa = sp.MoTa
                                    }).ToListAsync();
                return result;
            }
            else
            {
                var result = await (from sp in _context.SanPhams
                                    join ct in _context.LoaiSPs on sp.IDLoaiSP equals ct.ID
                                    join bt in _context.BienThes on sp.ID equals bt.IDSanPham
                                    where ct.ID == idloaisp
                                    select new SanPhamViewModel()
                                    {
                                        ID = sp.ID,
                                        Ten = sp.Ten,
                                        TrangThai = sp.TrangThai,
                                        LoaiSP = ct.Ten,
                                        MoTa = sp.MoTa
                                    }).ToListAsync();
                return result;
            }
        }
        // Hiển thị Sản phẩm và Biến thể qua IdSP
        public async Task<SanPhamViewModel> GetSanPhamById(Guid idsp)
        {
            var sanpham = await (from sp in _context.SanPhams
                                 join lsp in _context.LoaiSPs on sp.IDLoaiSP equals lsp.ID
                                 where sp.ID == idsp
                                 select new SanPhamViewModel()
                                 {
                                     ID = sp.ID,
                                     Ten = sp.Ten,
                                     MoTa = sp.MoTa,
                                     TrangThai = sp.TrangThai,
                                     LoaiSP = lsp.Ten,
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
                    // Check SP đã có TT này chưa
                    var ttspExist = await _context.ThuocTinhSanPhams.FirstOrDefaultAsync(c => c.IDSanPham == sanpham.ID && c.IDThuocTinh == id);
                    if (ttspExist == null) // Chưa có -> tạo SP-TT
                    {
                        ThuocTinhSanPham ttsp = new ThuocTinhSanPham()
                        {
                            ID = new Guid(),
                            IDSanPham = sanpham.ID,
                            IDThuocTinh = id,
                            NgayLuu = DateTime.Now,
                        };
                        await _context.ThuocTinhSanPhams.AddAsync(ttsp);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ttspExist.NgayLuu = DateTime.Now;
                        _context.ThuocTinhSanPhams.Update(ttspExist);
                        await _context.SaveChangesAsync();
                    }
                }
                // Xóa TT-SP mà sản phẩm không còn nữa
                var idTTSP = await _context.ThuocTinhSanPhams.OrderByDescending(c => c.NgayLuu).Take(request.ListIdThuocTinh.Count).Select(c => c.ID).ToListAsync();
                var lstdelete = await _context.ThuocTinhSanPhams.Where(c => !idTTSP.Contains(c.ID) && c.IDSanPham == sanpham.ID).ToListAsync();
                _context.ThuocTinhSanPhams.RemoveRange(lstdelete);
                await _context.SaveChangesAsync();
                return sanpham;
            }
            else // Ngược lại ->Tạo mới
            {
                SanPham sp = new SanPham()
                {
                    ID = new Guid(),
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
                        NgayLuu = DateTime.Now,
                    };
                    await _context.ThuocTinhSanPhams.AddAsync(ttsp);
                    await _context.SaveChangesAsync();
                }
                return sp;
            }
        }
        // XÓA SẢN PHẨM
        public async Task<bool> DeleteSanPham(Guid id)
        {
            try
            {
                var sp = await _context.SanPhams.FindAsync(id);
                // Xóa TT-SP
                var listTTSP = await _context.ThuocTinhSanPhams.Where(c => c.IDSanPham == id).ToListAsync();
                _context.ThuocTinhSanPhams.RemoveRange(listTTSP);
                await _context.SaveChangesAsync();
                // Xóa Biến thể : Xóa CTBT, Ảnh -> Xóa BT
                var listBT = await _context.BienThes.Where(c => c.IDSanPham == id).ToListAsync();
                foreach (var bt in listBT)
                {
                    //Xóa CTBT
                    var listCTBT = await _context.ChiTietBienThes.Where(c => c.IDBienThe == bt.ID).ToListAsync();
                    _context.ChiTietBienThes.RemoveRange(listCTBT);
                    await _context.SaveChangesAsync();
                    //Xóa Ảnh
                }
                _context.BienThes.RemoveRange(listBT);
                await _context.SaveChangesAsync();

                _context.SanPhams.Remove(sp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

    }
}
