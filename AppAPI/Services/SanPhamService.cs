using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AssignmentDBContext _context;
        public SanPhamService(AssignmentDBContext dBContext)
        {
            this._context = dBContext;
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
                                        join e in _context.LoaiSPs.Where(x => x.LoaiSPCha != null) on a.IDLoaiSP equals e.ID
                                        select new
                                        {
                                            ID = a.ID,
                                            Ten = a.Ten,
                                            TrangThai = a.TrangThai,
                                            TrangThaiCTSP = b.TrangThai,
                                            LoaiSP = e.Ten,
                                            IdChiTietSanPham = b.ID,
                                            Image = _context.Anhs.First(x => x.IDMauSac == b.IDMauSac && x.IDSanPham == a.ID).DuongDan,
                                            IDMauSac = b.IDMauSac,
                                            IDKichCo = b.IDKichCo,
                                            IDChatLieu = a.IDChatLieu,
                                            GiaGoc = b.GiaBan,
                                            IDKhuyenMai = b.IDKhuyenMai,
                                            NgayTao = b.NgayTao
                                        }).ToListAsync();
                var response = new List<SanPhamViewModel>();
                foreach (var item in lstSanPham)
                {
                    response.Add(new SanPhamViewModel()
                    {
                        ID = item.ID,
                        Ten = item.Ten,
                        TrangThai = item.TrangThai,
                        TrangThaiCTSP = item.TrangThai,
                        LoaiSP = item.LoaiSP,
                        IdChiTietSanPham = item.IdChiTietSanPham,
                        Image = item.Image,
                        IDMauSac = item.IDMauSac,
                        IDKichCo = item.IDKichCo,
                        IDChatLieu = item.IDChatLieu,
                        GiaGoc = item.GiaGoc,
                        GiaBan = item.IDKhuyenMai==null?item.GiaGoc:GetKhuyenMai(item.IDKhuyenMai,item.GiaGoc),
                        NgayTao = item.NgayTao
                    });
                }
                return response;
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
        public async Task<ChiTietSanPhamUpdateRequest> AddSanPham(SanPhamRequest request)
        {
            try
            {
                var first = true;
                List<ChiTietSanPhamRequest> lst = new List<ChiTietSanPhamRequest>();
                LoaiSP? loaiSPCon = _context.LoaiSPs.Where(x => x.IDLoaiSPCha != null).FirstOrDefault(x => x.Ten == request.TenLoaiSPCon);
                ChatLieu? chatLieu = _context.ChatLieus.FirstOrDefault(x => x.Ten == request.TenChatLieu);
                if (loaiSPCon == null)
                {
                    LoaiSP? loaiSPCha = _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null).FirstOrDefault(x => x.Ten == request.TenLoaiSPCha);
                    if (loaiSPCha == null)
                    {
                        loaiSPCha = new LoaiSP() { ID = Guid.NewGuid(), Ten = request.TenLoaiSPCha, TrangThai = 1 };
                        _context.LoaiSPs.AddAsync(loaiSPCha);
                    }
                    loaiSPCon = new LoaiSP() { ID = Guid.NewGuid(), Ten = request.TenLoaiSPCon, IDLoaiSPCha = loaiSPCha.ID, TrangThai = 1 };
                    await _context.LoaiSPs.AddAsync(loaiSPCon);
                }
                if (chatLieu == null)
                {
                    chatLieu = new ChatLieu() { ID = Guid.NewGuid(), Ten = request.TenChatLieu, TrangThai = 1 };
                    await _context.AddAsync(chatLieu);
                }
                var max = 0;
                if (_context.SanPhams.Any())
                {
                     max = _context.SanPhams.Max(x => Convert.ToInt32(x.Ma.Substring(2)));
                }
                SanPham sanPham = new SanPham() { ID = Guid.NewGuid(), Ten = request.Ten,Ma="SP"+(max+1),MoTa = request.MoTa, TrangThai = 1, TongDanhGia = 0, TongSoSao = 0, IDLoaiSP = loaiSPCon.ID, IDChatLieu = chatLieu.ID };
                await _context.SanPhams.AddAsync(sanPham);
                await _context.SaveChangesAsync();
                foreach (var x in request.MauSacs)
                {
                    foreach (var y in request.KichCos)
                    {
                        lst.Add(CreateChiTietSanPhamFromSanPham(x, y).Result);
                    }
                }
                return new ChiTietSanPhamUpdateRequest() { IDSanPham = sanPham.ID, ChiTietSanPhams = lst,Ma=sanPham.Ma };
            }
            catch { return new ChiTietSanPhamUpdateRequest(); }
        }
        public async Task<bool> AddAnhToSanPham(List<AnhRequest> request)
        {
            try
            {
                foreach (var x in request)
                {
                    Anh anh = new Anh() { ID = Guid.NewGuid(), DuongDan = x.DuongDan, IDSanPham = x.IDSanPham, IDMauSac = _context.MauSacs.First(y => y.Ma == x.MaMau).ID, TrangThai = 1 };
                    _context.Anhs.Add(anh);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Anh> GetAllAnhSanPham(Guid idSanPham)
        {
            return _context.Anhs.Where(x=>x.IDSanPham == idSanPham).ToList();
        }
        #endregion

        #region ChiTietSanPham
        public ChiTietSanPhamViewModel GetChiTietSanPhamByID(Guid id)
        {
            var temp = _context.ChiTietSanPhams.First(x => x.ID == id);
            var anh = _context.Anhs.FirstOrDefault(x => x.IDMauSac == temp.IDMauSac && x.IDSanPham == temp.IDSanPham);
            ChiTietSanPhamViewModel chiTietSanPham = new ChiTietSanPhamViewModel() { ID = temp.ID, Ten = _context.SanPhams.First(x => x.ID == temp.IDSanPham).Ten, SoLuong = temp.SoLuong, GiaBan = temp.IDKhuyenMai == null ? temp.GiaBan : GetKhuyenMai(temp.IDKhuyenMai.Value,temp.GiaBan), GiaGoc = temp.GiaBan, TrangThai = temp.TrangThai, Anh = anh!=null?anh.DuongDan:null, MauSac = _context.MauSacs.First(x => x.ID == temp.IDMauSac).Ten, KichCo = _context.KichCos.First(x => x.ID == temp.IDKichCo).Ten };
            return chiTietSanPham;
        }
        public async Task<ChiTietSanPhamViewModelHome> GetAllChiTietSanPhamHome(Guid idSanPham)
        {
            var sanPham = await _context.SanPhams.FindAsync(idSanPham);
            List<ChiTietSanPham> lstChiTietSanPham = _context.ChiTietSanPhams.Where(x => x.IDSanPham == idSanPham).ToList();
            List<MauSac> mauSacs = new List<MauSac>();
            List<KichCo> kichCos = new List<KichCo>();
            foreach (var x in lstChiTietSanPham)
            {
                mauSacs.Add(_context.MauSacs.FindAsync(x.IDMauSac).Result);
                kichCos.Add(_context.KichCos.FindAsync(x.IDKichCo).Result);
            }
            ChiTietSanPhamViewModelHome chiTietSanPham = new ChiTietSanPhamViewModelHome();
            chiTietSanPham.IDSanPham = idSanPham;
            chiTietSanPham.Ten = sanPham.Ten;
            chiTietSanPham.Anhs = new List<AnhRequest>();
            chiTietSanPham.MauSacs = new List<GiaTriViewModel>();
            chiTietSanPham.KichCos = new List<GiaTriViewModel>();
            chiTietSanPham.ChiTietSanPhams = new List<ChiTietSanPhamViewModel>();
            foreach (var item in _context.Anhs.Where(x => x.IDSanPham == idSanPham).ToList())
            {
                chiTietSanPham.Anhs.Add(new AnhRequest() { DuongDan = item.DuongDan, MaMau = item.IDMauSac.ToString() });
            }
            foreach (var item in mauSacs.Distinct().ToList())
            {
                chiTietSanPham.MauSacs.Add(new GiaTriViewModel() { GiaTri=item.Ma,ID = item.ID.Value});
            }
            foreach (var item in kichCos.Distinct().ToList())
            {
                chiTietSanPham.KichCos.Add(new GiaTriViewModel() { GiaTri = item.Ten, ID = item.ID });
            }
            foreach (var item in lstChiTietSanPham)
            {
                chiTietSanPham.ChiTietSanPhams.Add(new ChiTietSanPhamViewModel() { ID = item.ID,Ten=sanPham.Ten,SoLuong = item.SoLuong, GiaBan = item.IDKhuyenMai == null ? item.GiaBan : item.GiaBan - GetKhuyenMai(item.IDKhuyenMai.Value, item.GiaBan), GiaGoc = item.GiaBan,MauSac=item.IDMauSac.ToString(),KichCo=item.IDKichCo.ToString(),TrangThai=item.TrangThai});
            }
            chiTietSanPham.MoTa = sanPham.MoTa;
            var query = await (from sp in _context.SanPhams.Where(p => p.ID == idSanPham)
                               join ctsp in _context.ChiTietSanPhams on sp.ID equals ctsp.IDSanPham
                               join cthd in _context.ChiTietHoaDons on ctsp.ID equals cthd.IDCTSP
                               join dg in _context.DanhGias.Where(p=>p.TrangThai == 1) on cthd.ID equals dg.ID
                               select new DanhGiaViewModel()
                               {
                                   Sao = dg.Sao,
                               }).ToListAsync();
         
            foreach (var item in query)
            {
                chiTietSanPham.SoSao += Convert.ToInt32(item.Sao);
            }
            var sptt = await (from sp in _context.SanPhams.Where(p => p.IDLoaiSP == sanPham.IDLoaiSP && p.ID != idSanPham)
                               join ctsp in _context.ChiTietSanPhams.Where(p=>p.TrangThai == 1) on sp.ID equals ctsp.IDSanPham
                               join ms in _context.MauSacs on ctsp.IDMauSac equals ms.ID
                               
                               select new SanPhamTuongTuViewModel()
                               {
                                   IDSP = sp.ID,
                                   TenSP = sp.Ten,
                                   GiaSPTT = ctsp.GiaBan,
                                   DuongDanSPTT = _context.Anhs.FirstOrDefault(x => x.IDMauSac == ms.ID && x.IDSanPham == sp.ID).DuongDan,
                               }).Take(10).ToListAsync();
            chiTietSanPham.SoSao = chiTietSanPham.SoSao / query.Count();
            chiTietSanPham.sosaoPercent = float.IsNaN(chiTietSanPham.SoSao) ? 0 : Convert.ToInt32((chiTietSanPham.SoSao / 5) * 100);
            chiTietSanPham.SoDanhGia = query.Count();
            chiTietSanPham.LSTSPTuongTu = sptt;
            return chiTietSanPham;
        }
        public Task<bool> UpdateChiTietSanPham(ChiTietSanPham chiTietSanPham)
        {
            throw new NotImplementedException();
        }
        public async Task<ChiTietSanPhamRequest> CreateChiTietSanPhamFromSanPham(MauSac mauSacRequest, string tenKichCo)
        {
            var mauSac = _context.MauSacs.FirstOrDefault(x => x.Ma == mauSacRequest.Ma);
            if (mauSac == null)
            {
                mauSac = new MauSac() { ID = Guid.NewGuid(), Ten = mauSacRequest.Ten, Ma = mauSacRequest.Ma.ToLower(), TrangThai = 1 };
                _context.Add(mauSac);
            }
            var kichCo = _context.KichCos.FirstOrDefault(x => x.Ten == tenKichCo);
            if (kichCo == null)
            {
                kichCo = new KichCo() { ID = Guid.NewGuid(), Ten = tenKichCo, TrangThai = 1 };
                _context.Add(kichCo);

            }
            _context.SaveChanges();
            var chiTietSanPham = new ChiTietSanPhamRequest() { IDChiTietSanPham = Guid.NewGuid(), SoLuong = 0, GiaBan = 0, IDMauSac = mauSac.ID.Value, IDKichCo = kichCo.ID, MaMau = mauSac.Ma, TenMauSac = mauSac.Ten, TenKichCo = kichCo.Ten };
            return chiTietSanPham;
        }
        public async Task<bool> AddChiTietSanPhamFromSanPham(ChiTietSanPhamUpdateRequest request)
        {
            try
            {
                var tempTrangThai = new Guid(request.TrangThai);
                foreach (var x in request.ChiTietSanPhams)
                {
                    _context.ChiTietSanPhams.Add(new ChiTietSanPham() { ID = x.IDChiTietSanPham,SoLuong = x.SoLuong, GiaBan = x.GiaBan, NgayTao = DateTime.Now, TrangThai = x.IDChiTietSanPham == tempTrangThai ? 1 : 2, IDSanPham = request.IDSanPham, IDMauSac = x.IDMauSac, IDKichCo = x.IDKichCo,Ma=request.Ma+x.TenMauSac.Trim().ToUpper()+x.TenKichCo.ToUpper() });
                }
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<ChiTietSanPhamViewModelAdmin>> GetAllChiTietSanPhamAdmin(Guid idSanPham)
        {
            try
            {
                var lstChiTietSanPham = await (from a in _context.ChiTietSanPhams.Where(x => x.IDSanPham == idSanPham)
                                               join b in _context.MauSacs on a.IDMauSac equals b.ID
                                               join c in _context.KichCos on a.IDKichCo equals c.ID
                                               select new ChiTietSanPhamViewModelAdmin()
                                               {
                                                   MaMauSac = b.Ma,
                                                   TenKichCo = c.Ten,
                                                   SoLuong = a.SoLuong,
                                                   GiaBan = a.GiaBan,
                                                   NgayTao = a.NgayTao,
                                                   TenKhuyenMai = a.IDKhuyenMai == null ? "Ko" : _context.KhuyenMais.First(x => x.ID == a.IDKhuyenMai).Ten
                                               }).ToListAsync();
                return lstChiTietSanPham;
            }
            catch
            {
                return new List<ChiTietSanPhamViewModelAdmin>();
            }
        }
        public async Task<List<ChiTietSanPhamViewModel>> GetAllChiTietSanPham()
        {
            try
            {
                return await (from sp in _context.SanPhams.AsNoTracking()
                              join ctsp in _context.ChiTietSanPhams.AsNoTracking()
                              on sp.ID equals ctsp.IDSanPham
                              select new ChiTietSanPhamViewModel()
                              {
                                  ID = ctsp.ID,
                                  Ten = sp.Ten,
                                  GiaBan = ctsp.GiaBan,
                                  GiaGoc = ctsp.GiaBan,
                                  TrangThai = ctsp.TrangThai,
                                  SoLuong = ctsp.SoLuong,
                              }).ToListAsync();
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
            return await _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null).ToListAsync();
        }
        public async Task<List<LoaiSP>> GetAllLoaiSPCon(string tenLoaiSPCha)
        {
            var loaiSPCha = await _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null).FirstAsync(x => x.Ten == tenLoaiSPCha);
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

        public Task<List<ChiTietSanPham>> GetAllChiTietSanPham(Guid idSanPham)
        {
            throw new NotImplementedException();
        }
        public int GetKhuyenMai(Guid? idKhuyenMai,int giaSP)
        {
            var tienKhuyenMai = giaSP;
            var khuyenMai = _context.KhuyenMais.First(x => x.ID == idKhuyenMai);
            if (khuyenMai.TrangThai == 0 && khuyenMai.NgayKetThuc > DateTime.Now)
            {
                tienKhuyenMai -= khuyenMai.GiaTri;
            }
            else if (khuyenMai.TrangThai == 1 && khuyenMai.NgayKetThuc > DateTime.Now)
            {
                tienKhuyenMai -= (khuyenMai.GiaTri * giaSP) / 100;
            }
            return tienKhuyenMai;
        }
        #endregion
        //Nhinh thêm
        #region SanPhamBanHang
        public async Task<List<SanPhamBanHang>> GetAllSanPhamTaiQuay()
        {
            return await (from sp in _context.SanPhams.AsNoTracking()
                          let anh = _context.Anhs.AsNoTracking().Where(c => c.IDSanPham == sp.ID && c.DuongDan != null).FirstOrDefault()
                          let gia = _context.ChiTietSanPhams.AsNoTracking().Where(x => x.IDSanPham == sp.ID && x.GiaBan != null).FirstOrDefault()
                          where sp.TrangThai == 1
                          select new SanPhamBanHang()
                          {
                              Id = sp.ID,
                              Ten = sp.Ten,
                              MaSP = sp.Ma,
                              Anh = anh != null ? anh.DuongDan : null,
                              GiaBan = gia != null ? gia.GiaBan : 0,
                              IdLsp = sp.IDLoaiSP,
                          }).OrderBy(c=> c.MaSP).ToListAsync();
        }

        public async Task<ChiTietSanPhamBanHang> GetChiTietSPBHById(Guid idsp)
        {
            var lstMS = (from ctsp in _context.ChiTietSanPhams.AsNoTracking()
                         join ms in _context.MauSacs.AsNoTracking() on ctsp.IDMauSac equals ms.ID
                         where ctsp.IDSanPham == idsp
                         select new MauSac
                         {
                             ID = ms.ID,
                             Ma = ms.Ma,
                             Ten = ms.Ten,
                         }).Distinct().ToList();

            var lstKC = (from ctsp in _context.ChiTietSanPhams
                         join kc in _context.KichCos on ctsp.IDKichCo equals kc.ID
                         where ctsp.IDSanPham == idsp
                         select new KichCo
                         {
                             ID = kc.ID,
                             Ten = kc.Ten,
                         }).Distinct().ToList();

            var result = await (from sp in _context.SanPhams.AsNoTracking()
                                where sp.ID == idsp
                                select new ChiTietSanPhamBanHang()
                                {
                                    Id = sp.ID,
                                    Ten = sp.Ten,
                                    lstMau = lstMS,
                                    lstKC = lstKC,
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ChiTietCTSPBanHang>> GetChiTietCTSPBanHang(Guid idsp)
        {
            return await (from ctsp in _context.ChiTietSanPhams
                          join ms in _context.MauSacs on ctsp.IDMauSac equals ms.ID
                          join kc in _context.KichCos on ctsp.IDKichCo equals kc.ID
                          join sp in _context.SanPhams on ctsp.IDSanPham equals sp.ID
                          join km in _context.KhuyenMais on ctsp.IDKhuyenMai equals km.ID
                          into kmGroup
                          from km in kmGroup.DefaultIfEmpty()
                          where ctsp.IDSanPham == idsp
                          select new ChiTietCTSPBanHang()
                          {
                              Id = ctsp.ID,
                              Ten = sp.Ten,
                              ChiTiet = ms.Ten + " - " + kc.Ten,
                              idMauSac = ctsp.IDMauSac,
                              idKichCo = ctsp.IDKichCo,
                              SoLuong = ctsp.SoLuong,
                              Anh = (from ms in _context.MauSacs
                                     join a in _context.Anhs on ms.ID equals a.IDMauSac
                                     where ms.ID == ctsp.IDMauSac && a.IDSanPham == ctsp.IDSanPham
                                     select a).FirstOrDefault().DuongDan,
                              GiaGoc = ctsp.GiaBan,
                              GiaBan = km.TrangThai == null ? ctsp.GiaBan : (km.TrangThai == 0 ? ctsp.GiaBan - km.GiaTri : (ctsp.GiaBan * (100 - km.GiaTri) / 100)),
                          }).OrderByDescending(c=>c.ChiTiet).ToListAsync();
        }
        #endregion
    }
}
