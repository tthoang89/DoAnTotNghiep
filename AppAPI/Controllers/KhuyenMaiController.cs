using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiServices _khuyenmai;
      
        public KhuyenMaiController(IKhuyenMaiServices khuyenmai)
        {
          this._khuyenmai = khuyenmai;
           

        }

        // GET: api/<KhuyenMaiController>
        [HttpGet]
        public List<KhuyenMai> Get()
        {
            return _khuyenmai.GetAll();
        }
        [Route("GetAllBienThe")]
        [HttpGet]
        public List<ChiTietSanPham> GetAllBienThe()
        {
            return _khuyenmai.GetAllBienThe();
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
        [Route("Add1km1BT")]
        [HttpPut]
        public bool Add1KMVo1BT(Guid id,Guid IdKhuyenMai)
        {
            return _khuyenmai.Ad1KMVo1BT(id, IdKhuyenMai);
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
