using AppData.Models;

namespace AppAPI.IServices
{
    public interface IVoucherServices
    {
        public bool Add(string ten,int hinhthucgiamgia,int sotiencan,int giatri,DateTime NgayApDung,DateTime NgayKetThuc,int soluong,string mota,int trangthai);
        public bool Update(Guid Id, string ten, int hinhthucgiamgia, int sotiencan, int giatri, DateTime NgayApDung, DateTime NgayKetThuc, int soluong, string mota, int trangthai);
        public bool Delete(Guid Id);
        public Voucher GetById(Guid Id);
        public List<Voucher> GetAll();
    }
}
