using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppAPI.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AssignmentDBContext _context;
        public SanPhamService()
        {
            this._context = new AssignmentDBContext();
        }

        #region SanPham
        public Task<bool> UpdateSanPham(SanPhamRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<List<SanPhamViewModel>> TimKiemSanPham(SanPhamTimKiemNangCao sp)
        {
            throw new NotImplementedException();
        }
        public Task<List<SanPhamViewModel>> GetSanPhamByIdDanhMuc(Guid idloaisp)
        {
            throw new NotImplementedException();
        }
        public Task<SanPhamDetail> GetSanPhamById(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<SanPhamViewModel>> GetAllSanPham()
        {
            try
            {
                var lstSanPham = await (from a in _context.SanPhams.Where(x => x.TrangThai == 1)
                                        join b in _context.ChiTietSanPhams.Where(x => x.TrangThai == 1) on a.ID equals b.IDSanPham
                                        //join c in _context.Anhs.Where(x => x.TrangThai == 1) on a.ID equals c.IDSanPham
                                        join e in _context.LoaiSPs.Where(x=>x.LoaiSPCha!=null) on a.IDLoaiSP equals e.ID
                                  select new SanPhamViewModel()
                                  {
                                      ID = a.ID,
                                      Ten = a.Ten,
                                      TrangThai = a.TrangThai,
                                      LoaiSP = e.Ten,
                                      IdChiTietSanPham = b.ID,
                                      Image = _context.Anhs.First(x=>x.IDMauSac==b.IDMauSac&&x.IDSanPham==a.ID).DuongDan,
                                      IDMauSac = b.IDMauSac,
                                      IDKichCo = b.IDKichCo,
                                      IDChatLieu = a.IDChatLieu,
                                      GiaGoc = b.GiaBan,
                                      NgayTao = b.NgayTao,
                                      GiaBan = b.IDKhuyenMai==null?b.GiaBan:b.GiaBan*(100-(_context.KhuyenMais.First(x=>x.ID==b.IDKhuyenMai).GiaTri))/100
                                  }).ToListAsync();
                return lstSanPham;
            }
            catch
            {
                return new List<SanPhamViewModel>();
            }
        }
        public bool CheckTrungTenSP(SanPhamRequest lsp)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteSanPham(Guid id)
        {
            try
            {
                var sanPham = await _context.SanPhams.FindAsync(id);
                sanPham.TrangThai = 0;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddSanPham(SanPhamRequest request)
        {
            try
            {
                LoaiSP? loaiSPCon = _context.LoaiSPs.Where(x=>x.IDLoaiSPCha!=null).FirstOrDefault(x => x.Ten == request.TenLoaiSPCon);
                KichCo? kichCo = _context.KichCos.FirstOrDefault(x => x.Ten == request.TenKichCo);
                ChatLieu? chatLieu = _context.ChatLieus.FirstOrDefault(x => x.Ten == request.TenChatLieu);
                MauSac? mauSac = _context.MauSacs.FirstOrDefault(x => x.Ma == request.MaMauSac);
                if (loaiSPCon == null) {
                    LoaiSP? loaiSPCha = _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null).FirstOrDefault(x => x.Ten == request.TenLoaiSPCha);
                    if(loaiSPCha == null)
                    {
                        loaiSPCha = new LoaiSP() { ID = Guid.NewGuid(), Ten = request.TenLoaiSPCha, TrangThai =1};
                        _context.LoaiSPs.AddAsync(loaiSPCha);
                    }
                    loaiSPCon = new LoaiSP() { ID = Guid.NewGuid(), Ten = request.TenLoaiSPCon, IDLoaiSPCha = loaiSPCha.ID, TrangThai = 1 };
                    await _context.LoaiSPs.AddAsync(loaiSPCon);
                }
                if (kichCo == null)
                {
                    kichCo = new KichCo() { ID = Guid.NewGuid(), Ten = request.TenKichCo, TrangThai = 1 };
                    await _context.KichCos.AddAsync(kichCo);
                }
                if (mauSac == null)
                {
                    mauSac = new MauSac() { ID = Guid.NewGuid(), Ten = request.TenMauSac, Ma = request.MaMauSac, TrangThai = 1 };
                    await _context.AddAsync(mauSac);
                }
                if (chatLieu == null)
                {
                    chatLieu = new ChatLieu() { ID = Guid.NewGuid(), Ten = request.TenChatLieu, TrangThai = 1 };
                    await _context.AddAsync(chatLieu);
                }
                SanPham sanPham = new SanPham() { ID = Guid.NewGuid(), Ten = request.Ten, MoTa = request.MoTa, TrangThai = 1, TongDanhGia = 0, TongSoSao = 0, IDLoaiSP = loaiSPCon.ID, IDChatLieu = chatLieu.ID };
                Anh anh = new Anh() { ID = Guid.NewGuid(), DuongDan = request.DuongDanAnh, IDSanPham = sanPham.ID, IDMauSac = mauSac.ID, TrangThai = 1 };
                ChiTietSanPham chiTietSanPham = new ChiTietSanPham() { ID = Guid.NewGuid(), SoLuong = request.SoLuong, GiaBan = request.Giaban, NgayTao = DateTime.Now, TrangThai = 1, IDSanPham = sanPham.ID, IDKichCo = kichCo.ID, IDMauSac = mauSac.ID.Value };
                await _context.SanPhams.AddAsync(sanPham);
                await _context.ChiTietSanPhams.AddAsync(chiTietSanPham);
                await _context.Anhs.AddAsync(anh);
                await _context.SaveChangesAsync();
                return true;
            }catch { return false; }
        }
        #endregion

        #region ChiTietSanPham
        public Task<bool> UpdateChiTietSanPham(ChiTietSanPham chiTietSanPham)
        {
            throw new NotImplementedException();
        }
        public async Task<List<MauSac>> AddChiTietSanPham(ChiTietSanPhamRequest chiTietSanPham)
        {
            try
            {
                List<MauSac> lstMauSac = new List<MauSac>();
                MauSac? mauSac;
                foreach(var x in chiTietSanPham.MauSacs)
                {
                    foreach(var y in chiTietSanPham.KichCos)
                    {
                        mauSac = AddChiTietSanPham(x, y, chiTietSanPham.IDSanPham);
                        if(mauSac != null) lstMauSac.Add(mauSac);
                        _context.SaveChanges();
                    }
                }
                return lstMauSac.Distinct().ToList();
            }
            catch
            {
                return new List<MauSac>();
            }
        }
        public async Task<List<ChiTietSanPhamViewModel>> GetAllChiTietSanPham(Guid idSanPham)
        {
            try
            {
                var lstChiTietSanPham = await (from a in _context.ChiTietSanPhams.Where(x=>x.IDSanPham==idSanPham)
                                         join b in _context.MauSacs on a.IDMauSac equals b.ID
                                         join c in _context.KichCos on a.IDKichCo equals c.ID
                                         select new ChiTietSanPhamViewModel()
                                         {
                                             MaMauSac = b.Ma,
                                             TenKichCo = c.Ten,
                                             SoLuong = a.SoLuong,
                                             GiaBan = a.GiaBan,
                                             NgayTao =a.NgayTao,
                                             TenKhuyenMai = a.IDKhuyenMai==null?"Ko":_context.KhuyenMais.First(x=>x.ID==a.IDKhuyenMai).Ten
                                         }).ToListAsync();
                return lstChiTietSanPham;
            }
            catch
            {
                return new List<ChiTietSanPhamViewModel>();
            }
        }
        public async Task<bool> DeleteChiTietSanPham(Guid id)
        {
            try
            {
                var chiTietSanPham = await _context.ChiTietSanPhams.FindAsync(id);
                chiTietSanPham.TrangThai = 0;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public MauSac? AddChiTietSanPham(MauSac mauSacRequest, string tenKichCo,Guid idSanPham)
        {
            var temp = 0;
            var mauSac = _context.MauSacs.FirstOrDefault(x => x.Ma == mauSacRequest.Ma);
            if (mauSac == null)
            {
                mauSac = new MauSac() { ID = Guid.NewGuid(), Ten = mauSacRequest.Ten, Ma = mauSacRequest.Ma, TrangThai = 1 };
                _context.Add(mauSac);
                temp++;
            }
            var kichCo = _context.KichCos.FirstOrDefault(x => x.Ten == tenKichCo);
            if (kichCo == null)
            {
                kichCo = new KichCo() { ID = Guid.NewGuid(), Ten = tenKichCo, TrangThai = 1 };
                _context.Add(kichCo);
                temp++;
            }
            if (temp > 0)
            {
                var chiTietSanPham = new ChiTietSanPham() { ID = Guid.NewGuid(), SoLuong = 0, GiaBan = 0, NgayTao = DateTime.Now, TrangThai = 2, IDSanPham = idSanPham, IDMauSac = mauSac.ID.Value, IDKichCo = kichCo.ID };
                _context.Add(chiTietSanPham);
                return mauSac;
            }
            else if (!_context.ChiTietSanPhams.Any(x => x.IDMauSac == mauSac.ID && x.IDKichCo == kichCo.ID && x.IDSanPham==idSanPham))
            {
                var chiTietSanPham = new ChiTietSanPham() { ID = Guid.NewGuid(), SoLuong = 0, GiaBan = 0, NgayTao = DateTime.Now, TrangThai = 2, IDSanPham = idSanPham, IDMauSac = mauSac.ID.Value, IDKichCo = kichCo.ID };
                _context.Add(chiTietSanPham);
                if (!_context.Anhs.Any(x => x.IDMauSac == mauSac.ID && x.IDSanPham == idSanPham)) return mauSac;
                else return null;
            }
            return null;
        }

        //public async Task<bool> IsExistChiTietSanPham(string maMauSac, string tenKichCo)
        //{
        //    var mauSac = await _context.MauSacs.FirstOrDefaultAsync(x => x.Ma == maMauSac);
        //    if (mauSac == null) return false;
        //    var kichCo = await _context.KichCos.FirstOrDefaultAsync(x => x.Ten == tenKichCo);
        //    if (kichCo == null) return false;
        //    return await _context.ChiTietSanPhams.AnyAsync(x => x.IDMauSac == mauSac.ID && x.IDKichCo == kichCo.ID);
        //}
        #endregion

        #region LoaiSP
        public Task<LoaiSP> SaveLoaiSP(LoaiSPRequest lsp)
        {
            throw new NotImplementedException();
        }
        public Task<LoaiSP> GetLoaiSPById(Guid id)
        {
            throw new NotImplementedException();
        }
        public bool CheckTrungLoaiSP(LoaiSPRequest lsp)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteLoaiSP(Guid id)
        {
            try
            {
                var loaiSP = await _context.LoaiSPs.FindAsync(id);
                loaiSP.TrangThai = 0;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<LoaiSP>> GetAllLoaiSPCha()
        {
            return await _context.LoaiSPs.Where(x=>x.IDLoaiSPCha==null).ToListAsync();
        }
        public async Task<List<LoaiSP>> GetAllLoaiSPCon(string tenLoaiSPCha)
        {
            var loaiSPCha = await _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null).FirstAsync(x=>x.Ten==tenLoaiSPCha);
            return await _context.LoaiSPs.Where(x => x.IDLoaiSPCha == loaiSPCha.ID).ToListAsync();
        }
        #endregion

        #region Other
        public async Task<List<MauSac>> GetAllMauSac()
        {
            return await _context.MauSacs.ToListAsync();
        }

        public async Task<List<KichCo>> GetAllKichCo()
        {
            return await _context.KichCos.ToListAsync();
        }
        public async Task<List<ChatLieu>> GetAllChatLieu()
        {
            return await _context.ChatLieus.ToListAsync();
        }
        #endregion

        #region SanPham
        //TÌM KIẾM NÂNG CAO : Tên, List Loại Sp, khoảng Giá
        //public async Task<List<SanPhamViewModel>> TimKiemSanPham(SanPhamTimKiemNangCao sptk)
        //{
        //    // Lấy ds Sản phẩm cùng loại sp của chúng
        //    var sanphams = (from sp in _context.SanPhams.AsNoTracking()
        //                    join bt in _context.BienThes.AsNoTracking() on sp.ID equals bt.IDSanPham
        //                    join lsp in _context.LoaiSPs.AsNoTracking() on sp.IDLoaiSP equals lsp.ID
        //                    select new { sp, bt, lsp });
        //    // Tìm tên
        //    if (!string.IsNullOrEmpty(sptk.KeyWord))
        //    {
        //        sanphams = sanphams.AsNoTracking().Where(c => c.sp.Ten.ToLower().Contains(sptk.KeyWord.ToLower()));
        //    }
        //    // Loại sp
        //    if (sptk.IdLoaiSP.Count() > 0)
        //    {
        //        sanphams = sanphams.AsNoTracking().Where(c => sptk.IdLoaiSP.Contains(c.sp.IDLoaiSP));
        //    }
        //    // Khoảng giá
        //    if (sptk.GiaMax != 0 && sptk.GiaMin != 0)
        //    {
        //        sanphams = sanphams.AsNoTracking().Where(c => c.bt.GiaBan >= sptk.GiaMin && c.bt.GiaBan <= sptk.GiaMax);
        //    }
        //    var result = await sanphams.AsNoTracking().Select(c => new SanPhamViewModel()
        //    {
        //        ID = c.sp.ID,
        //        Ten = c.sp.Ten,
        //        TrangThai = c.sp.TrangThai,
        //        LoaiSP = c.lsp.Ten,
        //        IdBT = c.bt.ID,
        //        Image = (from img in _context.Anhs.AsNoTracking()
        //                 join abt in _context.AnhBienThes.AsNoTracking()
        //                 on img.ID equals abt.IdAnh
        //                 where abt.IdBienThe == c.bt.ID
        //                 select img.Ten).FirstOrDefault(),
        //        GiaGoc = c.bt.GiaBan,
        //        GiaBan = c.bt.IDKhuyenMai == null ? c.bt.GiaBan : (from km in _context.KhuyenMais.AsNoTracking()
        //                                                           where km.ID == c.bt.IDKhuyenMai
        //                                                           select c.bt.GiaBan * (100 - km.GiaTri) / 100).FirstOrDefault()
        //    }).ToListAsync();
        //    return result;
        //}
        ////CHECK TRÙNG TÊN SP
        //public bool CheckTrungTenSP(SanPhamRequest lsp)
        //{
        //    if (!_context.SanPhams.AsNoTracking().Any(c => c.Ten == lsp.Ten && c.ID != lsp.ID))
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //// LẤY DS SẢN PHẨM
        //public async Task<List<SanPhamViewModel>> GetAllSanPham()
        //{
        //    var sanphams = await (from sp in _context.SanPhams.AsNoTracking()
        //                          join lsp in _context.LoaiSPs.AsNoTracking() on sp.IDLoaiSP equals lsp.ID
        //                          join bt in _context.BienThes.AsNoTracking() on sp.ID equals bt.IDSanPham
        //                          where sp.TrangThai == 1 && bt.IsDefault == true// Sản phẩm hoạt động
        //                          select new SanPhamViewModel()
        //                          {
        //                              ID = sp.ID,
        //                              Ten = sp.Ten,
        //                              TrangThai = sp.TrangThai,
        //                              LoaiSP = lsp.Ten,
        //                              IdBT = bt.ID,
        //                              Image = (from img in _context.Anhs.AsNoTracking()
        //                                       join abt in _context.AnhBienThes.AsNoTracking()
        //                                       on img.ID equals abt.IdAnh
        //                                       where abt.IdBienThe == bt.ID
        //                                       select img.Ten).FirstOrDefault(),
        //                              GiaGoc = bt.GiaBan,
        //                              GiaBan = bt.IDKhuyenMai == null ? bt.GiaBan : (from km in _context.KhuyenMais.AsNoTracking()
        //                                                                             where km.ID == bt.IDKhuyenMai
        //                                                                             select bt.GiaBan * (100 - km.GiaTri) / 100).FirstOrDefault()
        //                          }).ToListAsync();
        //    return sanphams;
        //}
        //// Lấy DS sản phẩm theo IdLsp
        //public async Task<List<SanPhamViewModel>> GetSanPhamByIdDanhMuc(Guid idloaisp)
        //{
        //    var lsp = await _context.LoaiSPs.FindAsync(idloaisp);
        //    if (lsp.IDLoaiSPCha == null) // Danh mục cha -> Lấy sp của toàn bộ danh mục con
        //    {
        //        // Loại sp con
        //        var lspc = await _context.LoaiSPs.AsNoTracking().Where(c => c.IDLoaiSPCha == lsp.ID).Select(c => c.ID).ToListAsync();

        //        var result = await (from sp in _context.SanPhams.AsNoTracking()
        //                            join cate in _context.LoaiSPs.AsNoTracking() on sp.IDLoaiSP equals cate.ID
        //                            join bt in _context.BienThes.AsNoTracking() on sp.ID equals bt.IDSanPham
        //                            where sp.TrangThai == 1 && bt.IsDefault == true
        //                                   && lspc.Contains(sp.IDLoaiSP)
        //                            select new SanPhamViewModel()
        //                            {
        //                                ID = sp.ID,
        //                                Ten = sp.Ten,
        //                                TrangThai = sp.TrangThai,
        //                                LoaiSP = cate.Ten,
        //                                IdBT = bt.ID,
        //                                Image = (from img in _context.Anhs.AsNoTracking()
        //                                         join abt in _context.AnhBienThes.AsNoTracking() on img.ID equals abt.IdAnh
        //                                         where abt.IdBienThe == bt.ID
        //                                         select img.Ten).FirstOrDefault(),
        //                                GiaGoc = bt.GiaBan,
        //                                GiaBan = bt.IDKhuyenMai == null ? bt.GiaBan : (from km in _context.KhuyenMais.AsNoTracking()
        //                                                                               where km.ID == bt.IDKhuyenMai
        //                                                                               select bt.GiaBan * (100 - km.GiaTri) / 100).FirstOrDefault()
        //                            }).ToListAsync();
        //        return result;
        //    }
        //    else
        //    {
        //        var result = await (from sp in _context.SanPhams.AsNoTracking()
        //                            join loaisp in _context.LoaiSPs.AsNoTracking() on sp.IDLoaiSP equals lsp.ID
        //                            join bt in _context.BienThes.AsNoTracking() on sp.ID equals bt.IDSanPham
        //                            where sp.TrangThai == 1 && bt.IsDefault == true// Sản phẩm hoạt động
        //                            && loaisp.ID == idloaisp
        //                            select new SanPhamViewModel()
        //                            {
        //                                ID = sp.ID,
        //                                Ten = sp.Ten,
        //                                TrangThai = sp.TrangThai,
        //                                LoaiSP = lsp.Ten,
        //                                IdBT = bt.ID,
        //                                Image = (from img in _context.Anhs.AsNoTracking()
        //                                         join abt in _context.AnhBienThes.AsNoTracking()
        //                                         on img.ID equals abt.IdAnh
        //                                         where abt.IdBienThe == bt.ID
        //                                         select img.Ten).FirstOrDefault(),
        //                                GiaGoc = bt.GiaBan,
        //                                GiaBan = bt.IDKhuyenMai == null ? bt.GiaBan : (from km in _context.KhuyenMais.AsNoTracking()
        //                                                                               where km.ID == bt.IDKhuyenMai
        //                                                                               select bt.GiaBan * (100 - km.GiaTri) / 100).FirstOrDefault()
        //                            }).ToListAsync();
        //        return result;
        //    }
        //}
        //// Hiển thị Sản phẩm và Biến thể qua IdSP
        //public async Task<SanPhamDetail> GetSanPhamById(Guid idsp)
        //{
        //    var sanpham = await (from sp in _context.SanPhams.AsNoTracking()
        //                         join lsp in _context.LoaiSPs.AsNoTracking() on sp.IDLoaiSP equals lsp.ID
        //                         where sp.ID == idsp
        //                         select new SanPhamDetail()
        //                         {
        //                             ID = sp.ID,
        //                             Ten = sp.Ten,
        //                             MoTa = sp.MoTa,
        //                             TrangThai = sp.TrangThai,
        //                             LoaiSP = lsp.Ten,
        //                             ThuocTinhs = (from tt in _context.ThuocTinhs.AsNoTracking()
        //                                           join ttsp in _context.ThuocTinhSanPhams.AsNoTracking()
        //                                           on tt.ID equals ttsp.IDThuocTinh
        //                                           where ttsp.IDSanPham == sp.ID
        //                                           select new ThuocTinhRequest()
        //                                           {
        //                                               ID = tt.ID,
        //                                               Ten = tt.Ten,
        //                                               GiaTriRequests = (from gt in _context.GiaTris.AsNoTracking()
        //                                                                 where gt.IdThuocTinh == tt.ID
        //                                                                 select new GiaTriRequest()
        //                                                                 {
        //                                                                     ID = gt.ID,
        //                                                                     Ten = gt.Ten
        //                                                                 }).ToList(),
        //                                           }).ToList(),
        //                             BienThes = (from bt in _context.BienThes.AsNoTracking()
        //                                         where bt.IDSanPham == sp.ID
        //                                         select new BienTheViewModel()
        //                                         {
        //                                             ID = bt.ID,
        //                                             Ten = sp.Ten,
        //                                             SoLuong = bt.SoLuong,
        //                                             GiaGoc = bt.GiaBan,
        //                                             GiaBan = bt.IDKhuyenMai == null ? bt.GiaBan : (from km in _context.KhuyenMais where bt.IDKhuyenMai == km.ID select ((100 - km.GiaTri) / 100) * bt.GiaBan).FirstOrDefault(),
        //                                             TrangThai = bt.TrangThai,
        //                                             Anh = (from img in _context.Anhs.AsNoTracking() join btimg in _context.AnhBienThes on img.ID equals btimg.IdAnh where btimg.IdBienThe == bt.ID select img.Ten).ToList(),
        //                                             GiaTris = (from gt in _context.GiaTris.AsNoTracking()
        //                                                        join ctbt in _context.ChiTietBienThes on gt.ID equals ctbt.IDGiaTri
        //                                                        where ctbt.IDBienThe == bt.ID
        //                                                        select new GiaTriRequest()
        //                                                        {
        //                                                            ID = gt.ID,
        //                                                            Ten = gt.Ten
        //                                                        }).ToList()
        //                                         }).ToList()
        //                         }).FirstOrDefaultAsync();
        //    return sanpham;
        //}
        //// TẠO HOẶC CẬP NHẬT SẢN PHẨM
        //public async Task<SanPham> SaveSanPham(SanPhamRequest request)
        //{
        //    //Kiểm tra tồn tại
        //    var sanpham = await _context.SanPhams.FindAsync(request.ID);
        //    if (sanpham != null)//Update
        //    {
        //        sanpham.MoTa = request.MoTa;
        //        sanpham.Ten = request.Ten;
        //        sanpham.IDLoaiSP = request.IDLoaiSP;
        //        sanpham.TrangThai = request.TrangThai;
        //        _context.SanPhams.Update(sanpham);
        //        await _context.SaveChangesAsync();

        //        //Thuộc tính
        //        foreach (var id in request.ListIdThuocTinh)
        //        {
        //            // Check SP đã có TT này chưa
        //            var ttspExist = await _context.ThuocTinhSanPhams.AsNoTracking().FirstOrDefaultAsync(c => c.IDSanPham == sanpham.ID && c.IDThuocTinh == id);
        //            if (ttspExist == null) // Chưa có -> tạo SP-TT
        //            {
        //                ThuocTinhSanPham ttsp = new ThuocTinhSanPham()
        //                {
        //                    ID = new Guid(),
        //                    IDSanPham = sanpham.ID,
        //                    IDThuocTinh = id,
        //                    NgayLuu = DateTime.Now,
        //                };
        //                await _context.ThuocTinhSanPhams.AddAsync(ttsp);
        //                await _context.SaveChangesAsync();
        //            }
        //            else
        //            {
        //                ttspExist.NgayLuu = DateTime.Now;
        //                _context.ThuocTinhSanPhams.Update(ttspExist);
        //                await _context.SaveChangesAsync();
        //            }
        //        }
        //        // Xóa TT-SP mà sản phẩm không còn nữa
        //        var idTTSPSD = await _context.ThuocTinhSanPhams.AsNoTracking().OrderByDescending(c => c.NgayLuu).Take(request.ListIdThuocTinh.Count).Select(c => c.ID).ToListAsync();
        //        var lstdelete = await _context.ThuocTinhSanPhams.AsNoTracking().Where(c => !idTTSPSD.Contains(c.ID) && c.IDSanPham == sanpham.ID).ToListAsync();
        //        _context.ThuocTinhSanPhams.RemoveRange(lstdelete);
        //        await _context.SaveChangesAsync();
        //        return sanpham;
        //    }
        //    else // Ngược lại ->Tạo mới
        //    {
        //        SanPham sp = new SanPham()
        //        {
        //            ID = new Guid(),
        //            Ten = request.Ten,
        //            MoTa = request.MoTa,
        //            TrangThai = request.TrangThai,
        //            IDLoaiSP = request.IDLoaiSP,
        //        };
        //        await _context.SanPhams.AddAsync(sp);
        //        await _context.SaveChangesAsync();

        //        foreach (var id in request.ListIdThuocTinh)
        //        {
        //            var thuoctinh = await _context.ThuocTinhs.FindAsync(id);
        //            ThuocTinhSanPham ttsp = new ThuocTinhSanPham()
        //            {
        //                IDSanPham = sp.ID,
        //                IDThuocTinh = id,
        //                NgayLuu = DateTime.Now,
        //            };
        //            await _context.ThuocTinhSanPhams.AddAsync(ttsp);
        //            await _context.SaveChangesAsync();
        //        }
        //        return sp;
        //    }
        //}
        //// XÓA SẢN PHẨM
        //public async Task<bool> DeleteSanPham(Guid id)
        //{
        //    try
        //    {
        //        var sp = await _context.SanPhams.FindAsync(id);
        //        // Xóa TT-SP
        //        var listTTSP = await _context.ThuocTinhSanPhams.AsNoTracking().Where(c => c.IDSanPham == id).ToListAsync();
        //        _context.ThuocTinhSanPhams.RemoveRange(listTTSP);
        //        await _context.SaveChangesAsync();
        //        // Xóa Biến thể : Xóa CTBT, Ảnh -> Xóa BT
        //        var listBT = await _context.BienThes.AsNoTracking().Where(c => c.IDSanPham == id).ToListAsync();
        //        foreach (var bt in listBT)
        //        {
        //            //Xóa CTBT
        //            var listCTBT = await _context.ChiTietBienThes.AsNoTracking().Where(c => c.IDBienThe == bt.ID).ToListAsync();
        //            _context.ChiTietBienThes.RemoveRange(listCTBT);
        //            await _context.SaveChangesAsync();
        //            //Xóa Ảnh
        //            var listanhbt = await _context.AnhBienThes.AsNoTracking().Where(c => c.IdBienThe == bt.ID).ToListAsync();
        //            _context.AnhBienThes.RemoveRange(listanhbt);
        //        }
        //        _context.BienThes.RemoveRange(listBT);
        //        await _context.SaveChangesAsync();

        //        _context.SanPhams.Remove(sp);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion

    }
}
