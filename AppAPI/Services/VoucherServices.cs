using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;

namespace AppAPI.Services
{
    public class VoucherServices : IVoucherServices
    {
        private readonly IAllRepository<Voucher> _allRepository;
        AssignmentDBContext context= new AssignmentDBContext();
        public VoucherServices()
        {
            _allRepository= new AllRepository<Voucher>(context,context.Vouchers);
        }
        public bool Add(string ten, int hinhthucgiamgia, int sotiencan, int giatri, DateTime NgayApDung, DateTime NgayKetThuc, int soluong, string mota, int trangthai)
        {
            var voucher= new Voucher();
            voucher.ID=Guid.NewGuid();
            voucher.Ten=ten;
            voucher.HinhThucGiamGia=hinhthucgiamgia;
            voucher.SoTienCan=sotiencan;
            voucher.GiaTri = giatri;
            voucher.NgayApDung=NgayApDung;
            voucher.NgayKetThuc=NgayKetThuc;
            if (voucher.NgayApDung > voucher.NgayKetThuc)
            {
                return false;
            }
            voucher.SoLuong=soluong;
            voucher.MoTa = mota;
            voucher.TrangThai=trangthai;
            return _allRepository.Add(voucher);
        }

        public bool Delete(Guid Id)
        {
            var voucher = _allRepository.GetAll().FirstOrDefault(x => x.ID == Id);
            if (voucher != null)
            {
               
                return _allRepository.Delete(voucher);
            }
            else
            {
                return false;
            }
        }

        public List<Voucher> GetAll()
        {
            return _allRepository.GetAll();
        }

        public Voucher GetById(Guid Id)
        {
            return _allRepository.GetAll().FirstOrDefault(x => x.ID == Id);
        }

        public bool Update(Guid Id, string ten, int hinhthucgiamgia, int sotiencan, int giatri, DateTime NgayApDung, DateTime NgayKetThuc, int soluong, string mota, int trangthai)
        {
            var voucher= _allRepository.GetAll().FirstOrDefault(x => x.ID == Id);
            if (voucher != null)
            {
                voucher.Ten = ten;
                voucher.HinhThucGiamGia = hinhthucgiamgia;
                voucher.SoTienCan = sotiencan;
                voucher.GiaTri = giatri;
                voucher.NgayApDung = NgayApDung;
                voucher.NgayKetThuc = NgayKetThuc;
                if (voucher.NgayApDung > voucher.NgayKetThuc)
                {
                    return false;
                }
                voucher.SoLuong = soluong;
                voucher.MoTa = mota;
                voucher.TrangThai = trangthai;
                return _allRepository.Update(voucher);
            }
            else
            {
                return false;
            }
        }
    }
}
