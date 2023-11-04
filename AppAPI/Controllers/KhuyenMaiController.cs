using AppAPI.IServices;
using AppAPI.Services;
using AppData.IRepositories;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiServices _khuyenmai;
        private readonly AssignmentDBContext _dbcontext;
      
        public KhuyenMaiController()
        {
            _dbcontext = new AssignmentDBContext();
            _khuyenmai = new KhuyenMaiServices();
        }

        // GET: api/<KhuyenMaiController>
        [HttpGet]
        public List<KhuyenMai> Get()
        {
            return _khuyenmai.GetAll();
        }
        [Route("GetAllCTSPBySP")]
        [HttpGet]
        public async Task<List<AllViewCTSP>> GetAllCTSPBySp(Guid idSanPham)
        {
            if (!_dbcontext.ChiTietSanPhams.Any(c => c.IDSanPham == idSanPham)) throw new Exception($" khong tim thay san pham co id:{idSanPham}");
            var AllCTSP = await (from CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking()
                                 join mausac in _dbcontext.MauSacs.AsNoTracking() on CTSP.IDMauSac equals mausac.ID
                                 join size in _dbcontext.KichCos.AsNoTracking() on CTSP.IDKichCo equals size.ID
                                 join sp in _dbcontext.SanPhams.AsNoTracking() on CTSP.IDSanPham equals sp.ID
                                 where CTSP.IDSanPham == idSanPham
                                 select new AllViewCTSP()
                                 {
                                     ID = CTSP.ID,                                                               
                                     TenSanPham = sp.Ten,
                                     IdKhuyenMai = (from km in _dbcontext.KhuyenMais where CTSP.IDKhuyenMai == km.ID select CTSP.IDKhuyenMai).FirstOrDefault(),
                                     TenMauSac = mausac.Ten,
                                     TenKichCo = size.Ten,                                    
                                     GiaGoc = CTSP.GiaBan,
                                     GiaKhuyenMai = CTSP.GiaBan,
                                    SoLuong = CTSP.SoLuong,
                                    NgayTao = CTSP.NgayTao,
                                     TrangThai = CTSP.TrangThai
                                 }).ToListAsync();
            return AllCTSP;
        }
        // TimCTSP Theo Id Khuyen Mai
        [Route("GetCTSPByIdKm")]
        [HttpGet]
        public async Task<List<AllViewCTSP>> GetAllCTSPByIdKhuyenMai(Guid idkm)
        {
            if (!_dbcontext.KhuyenMais.Any(c => c.ID == idkm)) throw new Exception($" khong tim thay san pham co id:{idkm}");
            var AllCTSP = await (from CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking()
                                 join mausac in _dbcontext.MauSacs.AsNoTracking() on CTSP.IDMauSac equals mausac.ID
                                 join size in _dbcontext.KichCos.AsNoTracking() on CTSP.IDKichCo equals size.ID
                                 join sp in _dbcontext.SanPhams.AsNoTracking() on CTSP.IDSanPham equals sp.ID
                                
                                 select new AllViewCTSP()
                                 {
                                     ID = CTSP.ID,
                                     TenSanPham = sp.Ten,
                                     IdKhuyenMai = (from km in _dbcontext.KhuyenMais where CTSP.IDKhuyenMai == km.ID select CTSP.IDKhuyenMai).FirstOrDefault(),
                                     TenMauSac = mausac.Ten,
                                     TenKichCo = size.Ten,
                                     GiaGoc = CTSP.GiaBan,
                                     GiaKhuyenMai = CTSP.GiaBan,
                                     SoLuong = CTSP.SoLuong,
                                     NgayTao = CTSP.NgayTao,
                                     TrangThai = CTSP.TrangThai
                                 }).Where(x=>x.IdKhuyenMai==idkm).ToListAsync();
            return AllCTSP;
        }

        [Route("GetAllSP")]
        [HttpGet]

        public async Task<List<AllViewSp>> GetAllSP()
        {
            var AllCTSP = await (from SP in _dbcontext.SanPhams.AsNoTracking()
                                 join anh in _dbcontext.Anhs.AsNoTracking() on SP.ID equals anh.IDSanPham
                                 join loaisp in _dbcontext.LoaiSPs.AsNoTracking() on SP.IDLoaiSP equals loaisp.ID
                                 join chatlieu in _dbcontext.ChatLieus.AsNoTracking() on SP.IDChatLieu equals chatlieu.ID
                                 join CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking() on SP.ID equals CTSP.IDSanPham
                                 select new AllViewSp()
                                 {
                                     ID = SP.ID,
                                     Ten = SP.Ten,
                                     MoTa = SP.MoTa,
                                     TenAnh = anh.DuongDan,
                                     IdKhuyenMai = (from km in _dbcontext.KhuyenMais where CTSP.IDKhuyenMai == km.ID select CTSP.IDKhuyenMai).FirstOrDefault(),
                                     TongSoSao = SP.TongSoSao,
                                     TongDanhGia = SP.TongDanhGia,
                                     GiaBan = CTSP.GiaBan,
                                     IDLoaiSP = loaisp.ID,
                                     IDLoaiSPCha = loaisp.IDLoaiSPCha,
                                     IDChatLieu = chatlieu.ID,
                                     TrangThai = CTSP.TrangThai
                                 }).ToListAsync();
            return AllCTSP;
        }
        [Route("GetAllSPByKhuyenMai")]
        [HttpGet]

        public async Task<List<AllViewSp>> GetAllSPByKhuyenMai(Guid idkm)
        {
            if (!_dbcontext.KhuyenMais.Any(c => c.ID == idkm)) throw new Exception($" khong tim thay san pham co id:{idkm}");
            var AllCTSP = await (from SP in _dbcontext.SanPhams.AsNoTracking()
                                 join anh in _dbcontext.Anhs.AsNoTracking() on SP.ID equals anh.IDSanPham
                                 join loaisp in _dbcontext.LoaiSPs.AsNoTracking() on SP.IDLoaiSP equals loaisp.ID
                                 join chatlieu in _dbcontext.ChatLieus.AsNoTracking() on SP.IDChatLieu equals chatlieu.ID
                                 join CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking() on SP.ID equals CTSP.IDSanPham
                                
                                 select new AllViewSp()
                                 {
                                     ID = SP.ID,
                                     Ten = SP.Ten,
                                     MoTa = SP.MoTa,
                                     TenAnh = anh.DuongDan,
                                     IdKhuyenMai = (from km in _dbcontext.KhuyenMais where CTSP.IDKhuyenMai == km.ID select CTSP.IDKhuyenMai).FirstOrDefault(),
                                     TongSoSao = SP.TongSoSao,
                                     TongDanhGia = SP.TongDanhGia,
                                     GiaBan = CTSP.GiaBan,
                                     IDLoaiSP = loaisp.ID,
                                     IDLoaiSPCha = loaisp.IDLoaiSPCha,
                                     IDChatLieu = chatlieu.ID,
                                     TrangThai = CTSP.TrangThai
                                 }).Where(x=>x.IdKhuyenMai==idkm).ToListAsync();
            return AllCTSP;
        }
        [Route("GetAllSPByKmLoaiSPChatLieu")]
        [HttpGet]

        public async Task<List<AllViewSp>> GetAllSPByKm(Guid? idkm,Guid? idLoaiSP,Guid? idChatLieu)
        {
            if (!_dbcontext.KhuyenMais.Any(c => c.ID == idkm)&& !_dbcontext.LoaiSPs.Any(c => c.ID == idLoaiSP) && !_dbcontext.ChatLieus.Any(y => y.ID == idChatLieu)) throw new Exception($" khong tim thay san pham co id:{idkm},{idLoaiSP},{idChatLieu}");
            var AllCTSP =  (from SP in _dbcontext.SanPhams.AsNoTracking()
                                 join anh in _dbcontext.Anhs.AsNoTracking() on SP.ID equals anh.IDSanPham
                                 join loaisp in _dbcontext.LoaiSPs.AsNoTracking() on SP.IDLoaiSP equals loaisp.ID
                                 join chatlieu in _dbcontext.ChatLieus.AsNoTracking() on SP.IDChatLieu equals chatlieu.ID
                                 join CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking() on SP.ID equals CTSP.IDSanPham
                                 join km in _dbcontext.KhuyenMais.AsNoTracking() on CTSP.IDKhuyenMai equals km.ID
                                 select new {SP,anh,loaisp, chatlieu, CTSP,km});
            // Tim Theo IdKhuyenMai
            if (!string.IsNullOrEmpty(idkm.ToString()))
            {
                AllCTSP = AllCTSP.AsNoTracking().Where(x => x.km.ID == idkm);
            }
            if (!string.IsNullOrEmpty(idLoaiSP.ToString()))
            {
                AllCTSP = AllCTSP.AsNoTracking().Where(x => x.loaisp.ID == idLoaiSP);
                
            }
            if (!string.IsNullOrEmpty(idChatLieu.ToString()))
            {
                AllCTSP = AllCTSP.AsNoTracking().Where(x => x.chatlieu.ID == idChatLieu);
            }
            var result = await AllCTSP.AsNoTracking().Select(c => new AllViewSp() { ID= c.SP.ID,
                Ten=c.SP.Ten,
                MoTa=c.SP.MoTa,
                TenAnh=c.anh.DuongDan,
                IdKhuyenMai=c.km.ID,
                TongSoSao=c.SP.TongSoSao,
                TongDanhGia=c.SP.TongDanhGia,
                GiaBan=c.CTSP.GiaBan,
                IDLoaiSP=c.SP.IDLoaiSP,
                IDLoaiSPCha=c.loaisp.IDLoaiSPCha,
                IDChatLieu=c.chatlieu.ID,
                TrangThai=c.CTSP.TrangThai

            }).ToListAsync();
            return result;          
        }
        

        // GET api/<KhuyenMaiController>/5
        [HttpGet("{id}")]
        public KhuyenMai Get(Guid id)
        {
            return _khuyenmai.GetById(id);
        }
        [Route("TimKiemTenKM")]
        [HttpGet]
        public List<KhuyenMai> GetByTen(string Ten)
        {
            return _khuyenmai.GetKMByName(Ten);
        }

        // POST api/<KhuyenMaiController>
        [HttpPost]
        public bool Post(KhuyenMaiView kmv)
        {
            return _khuyenmai.Add(kmv);
        }
        [Route("AddKmVoBT")]
        [HttpPut]
        // PUT api/<KhuyenMaiController>/5 
        public bool AddKMVoBienThe(List<Guid> bienThes,Guid IdKhuyenMai)
        {
            return _khuyenmai.AdKMVoBT(bienThes,IdKhuyenMai);
        }
        
        // Lam start
        [Route("XoaKmRaBT")]
        [HttpPut]
        // PUT api/<KhuyenMaiController>/5 
        public bool XoaKMRaBienThe(List<Guid> bienThes)
        {
            return _khuyenmai.XoaAllKMRaBT(bienThes);
        }
        //Lam end
        // PUT api/<KhuyenMaiController>/5
        [Route("UpdateKM")]
        [HttpPut]
        public bool Put(KhuyenMaiView kmv)
        {
            var khuyenmai = _khuyenmai.GetById(kmv.ID);
            if (khuyenmai != null)
            {
                return _khuyenmai.Update(kmv);  
            }
            else
            {
                return false;
            }
        }

        // DELETE api/<KhuyenMaiController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var khuyenmai = _khuyenmai.GetById(id);
            if (khuyenmai != null)
            {
                return _khuyenmai.Delete(khuyenmai.ID);
            }
            else
            {
                return false;
            }
        }
    }
}
