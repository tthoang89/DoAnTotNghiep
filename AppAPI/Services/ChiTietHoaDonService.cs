using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels.BanOffline;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly AssignmentDBContext _context;
        public ChiTietHoaDonService()
        {
            _context = new AssignmentDBContext();
        }

        public async Task<ChiTietHoaDon> SaveCTHoaDon(HoaDonChiTietRequest request)
        {
            // Kiểm tra sp tồn tại trong hóa đơn này chưa
            var CTSPexist = _context.ChiTietHoaDons.Where(c => c.IDHoaDon == request.IdHoaDon).Any(c => c.IDCTSP == request.IdChiTietSanPham);
            if (CTSPexist != true) //k tồn tại -> chưa có hdct-> tạo
            {
                var danhgia = new DanhGia()
                {
                    ID = request.Id,
                    TrangThai = 0,
                };
                await _context.DanhGias.AddAsync(danhgia);
                await _context.SaveChangesAsync();

                var hdct = new ChiTietHoaDon()
                {
                    ID = danhgia.ID,
                    IDHoaDon = request.IdHoaDon,
                    IDCTSP = request.IdChiTietSanPham,
                    SoLuong = request.SoLuong,
                    DonGia = request.DonGia,
                    TrangThai = 0,
                };
                await _context.ChiTietHoaDons.AddAsync(hdct);
                await _context.SaveChangesAsync();
                //Trừ số lượng CTSP
                var ctsp = _context.ChiTietSanPhams.Find(request.IdChiTietSanPham);
                ctsp.SoLuong -= request.SoLuong;
                _context.ChiTietSanPhams.Update(ctsp);
                await _context.SaveChangesAsync();
                return hdct;
            }
            else
            {
                var exist = _context.ChiTietHoaDons.Where(c => c.IDCTSP == request.IdChiTietSanPham && c.IDHoaDon == request.IdHoaDon).FirstOrDefault();
                var ctsp = _context.ChiTietSanPhams.Find(request.IdChiTietSanPham);
                exist.SoLuong += request.SoLuong;
                exist.DonGia = request.DonGia;
                _context.Update(exist);
                await _context.SaveChangesAsync();

                //Thay đổi số lượng bt
                ctsp.SoLuong -=request.SoLuong;
                _context.ChiTietSanPhams.Update(ctsp);
                await _context.SaveChangesAsync();
                return exist;
            }

        }

        public async Task<bool> DeleteCTHoaDon(Guid id)
        {
            var exist = _context.ChiTietHoaDons.Find(id);
            if (exist == null) throw new Exception($"Không tìm thấy CTHD: {id}");
            //Tăng lại số lượng cho sp
            var ctsp = await _context.ChiTietSanPhams.FindAsync(exist.IDCTSP);
            ctsp.SoLuong += exist.SoLuong;
            _context.ChiTietSanPhams.Update(ctsp);
            await _context.SaveChangesAsync();
            //Xóa đánh giá 
            var danhgia = await _context.DanhGias.Where(c => c.ID == id).FirstOrDefaultAsync();
            _context.DanhGias.Remove(danhgia);
            await _context.SaveChangesAsync();
            _context.ChiTietHoaDons.Remove(exist);
            await _context.SaveChangesAsync();
            return true;
        }

        public List<ChiTietHoaDon> GetAllCTHoaDon()
        {
            return _context.ChiTietHoaDons.ToList();
        }

        public async Task<List<HoaDonChiTietViewModel>> GetHDCTByIdHD(Guid idhd)
        {
            var x = (from cthd in _context.ChiTietHoaDons
                          join ctsp in _context.ChiTietSanPhams on cthd.IDCTSP equals ctsp.ID
                          join sp in _context.SanPhams on ctsp.IDSanPham equals sp.ID
                          where cthd.IDHoaDon == idhd
                          select new {cthd,ctsp,sp });
            var result = await(from cthd in _context.ChiTietHoaDons
                               join bt in _context.ChiTietSanPhams on cthd.IDCTSP equals bt.ID
                               join sp in _context.SanPhams on bt.IDSanPham equals sp.ID
                               where cthd.IDHoaDon == idhd
                               select new HoaDonChiTietViewModel()
                          {
                              Id = cthd.ID,
                              IdHoaDon = cthd.IDHoaDon,
                              IDChiTietSanPham = bt.ID,
                              Ten = sp.Ten,
                              SoLuong = cthd.SoLuong,
                              DonGia = cthd.DonGia,
                          }).ToListAsync();
            return result;
        }
    }
}
