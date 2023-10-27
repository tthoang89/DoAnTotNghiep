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
            var btexist = _context.ChiTietHoaDons.Where(c => c.IDHoaDon == request.IdHoaDon).Any(c => c.IDBienThe == request.IdBienThe);
            if (btexist != true) //k tồn tại -> chưa có hdct-> tạo
            {
                var hdct = new ChiTietHoaDon()
                {
                    ID = new Guid(),
                    IDHoaDon = request.IdHoaDon,
                    IDBienThe = request.IdBienThe,
                    SoLuong = request.SoLuong,
                    DonGia = request.DonGia,
                };
                await _context.ChiTietHoaDons.AddAsync(hdct);
                await _context.SaveChangesAsync();
                //Trừ số lượng bt
                var bt = _context.BienThes.Find(request.IdBienThe);
                bt.SoLuong -= request.SoLuong;
                _context.BienThes.Update(bt);
                await _context.SaveChangesAsync();
                return hdct;
            }
            else
            {
                var exist = _context.ChiTietHoaDons.Where(c => c.IDBienThe == request.IdBienThe && c.IDHoaDon == request.IdHoaDon).FirstOrDefault();
                var bt = _context.BienThes.Find(request.IdBienThe);
                exist.SoLuong += request.SoLuong;
                exist.DonGia = request.DonGia;
                _context.Update(exist);
                await _context.SaveChangesAsync();
                //Thay đổi số lượng bt
                bt.SoLuong -=request.SoLuong;
                _context.BienThes.Update(bt);
                await _context.SaveChangesAsync();
                return exist;
            }

        }

        public async Task<bool> DeleteCTHoaDon(Guid id)
        {
            var exist = _context.ChiTietHoaDons.Find(id);
            if (exist == null) throw new Exception($"Không tìm thấy CTHD: {id}");
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
                          join bt in _context.BienThes on cthd.IDBienThe equals bt.ID
                          join sp in _context.SanPhams on bt.IDSanPham equals sp.ID
                          where cthd.IDHoaDon == idhd
                          select new {cthd,bt,sp });
            var result = await(from cthd in _context.ChiTietHoaDons
                               join bt in _context.BienThes on cthd.IDBienThe equals bt.ID
                               join sp in _context.SanPhams on bt.IDSanPham equals sp.ID
                               where cthd.IDHoaDon == idhd
                               select new HoaDonChiTietViewModel()
                          {
                              Id = cthd.ID,
                              IdHoaDon = cthd.IDHoaDon,
                              IDBienThe = bt.ID,
                              Ten = sp.Ten,
                              SoLuong = cthd.SoLuong,
                              DonGia = cthd.DonGia,
                          }).ToListAsync();
            return result;
        }
    }
}
