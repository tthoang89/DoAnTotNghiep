using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using System.Security.Cryptography.Xml;

namespace AppAPI.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IAllRepository<HoaDon> reposHoaDon;
        private readonly IAllRepository<ChiTietHoaDon> reposChiTietHoaDon;
        private readonly IAllRepository<BienThe> reposBienThe;
        private readonly IAllRepository<Voucher> reposVoucher;
        private readonly IAllRepository<QuyDoiDiem> reposQuyDoiDiem;
        private readonly IAllRepository<LichSuTichDiem> reposLichSuTichDiem;
        private readonly IAllRepository<KhachHang> reposKhachHang;
        private readonly IAllRepository<SanPham> reposSanPham;


        AssignmentDBContext context = new AssignmentDBContext();

        public HoaDonService()
        {
            reposHoaDon = new AllRepository<HoaDon>(context, context.HoaDons);
            reposChiTietHoaDon = new AllRepository<ChiTietHoaDon>(context, context.ChiTietHoaDons);
            reposBienThe = new AllRepository<BienThe>(context, context.BienThes);
            reposVoucher = new AllRepository<Voucher>(context, context.Vouchers);
            reposQuyDoiDiem = new AllRepository<QuyDoiDiem>(context, context.QuyDoiDiems);
            reposLichSuTichDiem = new AllRepository<LichSuTichDiem>(context, context.LichSuTichDiems);
            reposKhachHang = new AllRepository<KhachHang>(context, context.KhachHangs);
            reposSanPham = new AllRepository<SanPham>(context, context.SanPhams);
            context = new AssignmentDBContext();
        }

        public int CheckVoucher(string ten, int tongtien)
        {
            var voucher = reposVoucher.GetAll().FirstOrDefault(p => p.Ten == ten);
            if (voucher != null)
            {
                if (tongtien >= voucher.SoTienCan && DateTime.Compare(voucher.NgayApDung, DateTime.Now) <= 0 && DateTime.Compare(DateTime.Now, voucher.NgayKetThuc) <= 0 && voucher.SoLuong > 0)
                {
                    if (voucher.HinhThucGiamGia == 1)
                    {
                        tongtien -= voucher.GiaTri;
                        return tongtien;
                    }
                    else
                    {
                        tongtien = tongtien - (tongtien * voucher.GiaTri / 100);
                        return tongtien;
                    }

                }
                else
                {
                    return tongtien;
                }
            }
            else
            {
                return tongtien;
            }
        }

        //Thanh toan online
        public bool CreateHoaDon(List<ChiTietHoaDonViewModel> chiTietHoaDons, HoaDonViewModel hoaDon)
        {
            try
            {
                var voucher = reposVoucher.GetAll().FirstOrDefault(p => p.Ten == hoaDon.TenVoucher);
                if (chiTietHoaDons != null)
                {
                    HoaDon hoaDon1 = new HoaDon();
                    hoaDon1.ID = Guid.NewGuid();
                    hoaDon1.IDNhanVien = null;
                    if (voucher != null)
                    {
                        hoaDon1.IDVoucher = voucher.ID;
                        voucher.SoLuong--;
                        reposVoucher.Update(voucher);
                    }
                    else
                    {
                        hoaDon1.IDVoucher = null;
                    }
                    hoaDon1.TenNguoiNhan = hoaDon.Ten;
                    hoaDon1.SDT = hoaDon.SDT;
                    hoaDon1.Email = hoaDon.Email;
                    hoaDon1.NgayTao = DateTime.Now;
                    hoaDon1.DiaChi = hoaDon.DiaChi;
                    hoaDon1.TienShip = hoaDon.TienShip;
                    hoaDon1.PhuongThucThanhToan = hoaDon.PhuongThucThanhToan;
                    hoaDon1.TrangThaiGiaoHang = 2;
                    if (reposHoaDon.Add(hoaDon1))
                    {
                        foreach (var x in chiTietHoaDons)
                        {
                            ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
                            chiTietHoaDon.ID = Guid.NewGuid();
                            chiTietHoaDon.IDHoaDon = hoaDon1.ID;
                            chiTietHoaDon.IDBienThe = x.IDBienThe;
                            chiTietHoaDon.SoLuong = x.SoLuong;
                            chiTietHoaDon.DonGia = x.DonGia;
                            chiTietHoaDon.TrangThai = 1;
                            reposChiTietHoaDon.Add(chiTietHoaDon);
                            var bienThe = reposBienThe.GetAll().FirstOrDefault(p => p.ID == x.IDBienThe);
                            bienThe.SoLuong -= chiTietHoaDon.SoLuong;
                            reposBienThe.Update(bienThe);
                        }
                        //tích điểm, dùng điểm
                        if (hoaDon.IDNguoiDung != null)
                        {
                            QuyDoiDiem quyDoiDiem = reposQuyDoiDiem.GetAll().First();
                            KhachHang khachHang = reposKhachHang.GetAll().FirstOrDefault(p => p.IDKhachHang == hoaDon.IDNguoiDung);
                            if (hoaDon.Diem == 0)
                            {
                                khachHang.DiemTich += hoaDon.TongTien / quyDoiDiem.TiLeTichDiem;
                                reposKhachHang.Update(khachHang);
                                LichSuTichDiem lichSuTichDiem = new LichSuTichDiem()
                                {
                                    ID = Guid.NewGuid(),
                                    IDKhachHang = hoaDon.IDNguoiDung,
                                    IDQuyDoiDiem = quyDoiDiem.ID,
                                    IDHoaDon = hoaDon1.ID,
                                    Diem = hoaDon.TongTien / quyDoiDiem.TiLeTichDiem,
                                    TrangThai = 1
                                };
                                reposLichSuTichDiem.Add(lichSuTichDiem);
                            }
                            else
                            {
                                if (khachHang.DiemTich >= hoaDon.Diem)
                                {
                                    khachHang.DiemTich = khachHang.DiemTich - hoaDon.Diem;
                                    reposKhachHang.Update(khachHang);
                                    LichSuTichDiem lichSuTichDiem = new LichSuTichDiem()
                                    {
                                        ID = Guid.NewGuid(),
                                        IDKhachHang = hoaDon.IDNguoiDung,
                                        IDQuyDoiDiem = quyDoiDiem.ID,
                                        IDHoaDon = hoaDon1.ID,
                                        Diem = hoaDon.Diem,
                                        TrangThai = 0
                                    };
                                    reposLichSuTichDiem.Add(lichSuTichDiem);
                                }
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                { 
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
       

        public bool CreateHoaDonOffline(HoaDonNhap hdnhap)
        {
            try
            {
                HoaDon hoaDon1 = new HoaDon();
                hoaDon1.ID = Guid.NewGuid();
                hoaDon1.IDNhanVien = hdnhap.IdNhanVien;
                hoaDon1.NgayTao = DateTime.Now;
                hoaDon1.TrangThaiGiaoHang = 1;
                hoaDon1.TienShip = 0;
                if (reposHoaDon.Add(hoaDon1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteHoaDon(Guid id)
        {
            HoaDon hoaDon = reposHoaDon.GetAll().FirstOrDefault(p => p.ID == id);
            var lsthdct = reposChiTietHoaDon.GetAll().Where(c => c.IDHoaDon == hoaDon.ID).ToList();
            context.ChiTietHoaDons.RemoveRange(lsthdct);
            //Cộng lại số lượng sản phẩm 
            context.SaveChanges();
            return reposHoaDon.Delete(hoaDon);
        }

        public List<ChiTietHoaDon> GetAllChiTietHoaDon(Guid idHoaDon)
        {
            return reposChiTietHoaDon.GetAll().Where(x => x.IDHoaDon == idHoaDon).ToList();
        }

        public List<HoaDon> GetAllHoaDon()
        {
            return reposHoaDon.GetAll();
        }

        
        public List<HoaDon> LichSuGiaoDich(Guid idNguoiDung)
        {
            var idhoadon = reposLichSuTichDiem.GetAll().Where(p => p.IDKhachHang == idNguoiDung).ToList();
            List<HoaDon> lichSuGiaoDich = new List<HoaDon>();
            foreach (var item in idhoadon)
            {
                lichSuGiaoDich.Add(reposHoaDon.GetAll().FirstOrDefault(p => p.ID == item.IDHoaDon));
            }
            return lichSuGiaoDich;
        }

        public List<HoaDon> TimKiemVaLocHoaDon(string ten, int? loc)
        {
            List<HoaDon> timkiem = reposHoaDon.GetAll().Where(p => p.TenNguoiNhan.ToLower().Contains(ten.ToLower())).ToList();
            if (loc == 0)
            {
                List<HoaDon> locTangDan = timkiem.OrderBy(p => p.NgayTao).ToList();
                return locTangDan;
            }
            else if (loc == 1)
            {
                List<HoaDon> locGiamDan = timkiem.OrderByDescending(p => p.NgayTao).ToList();
                return locGiamDan;
            }
            return timkiem;
        }

        public bool UpdateHoaDon(HoaDon hoaDon)
        {
            var update = reposHoaDon.GetAll().FirstOrDefault(p => p.ID == hoaDon.ID);
            update.IDNhanVien = null;
            update.IDVoucher = null;
            update.TenNguoiNhan = hoaDon.TenNguoiNhan;
            update.SDT = hoaDon.SDT;
            update.Email = hoaDon.Email;
            update.NgayTao = hoaDon.NgayTao;
            update.DiaChi = hoaDon.DiaChi;
            update.TienShip = hoaDon.TienShip;
            update.PhuongThucThanhToan = hoaDon.PhuongThucThanhToan;
            update.TrangThaiGiaoHang = hoaDon.TrangThaiGiaoHang;
            return reposHoaDon.Update(update);

        }

        public bool UpdateTrangThaiGiaoHang(Guid idHoaDon, int trangThai, Guid idNhanVien)
        {
            var update = reposHoaDon.GetAll().FirstOrDefault(p => p.ID == idHoaDon);
            List<ChiTietHoaDon> chitiethoadon = reposChiTietHoaDon.GetAll().Where(p => p.IDHoaDon == idHoaDon).ToList();
            if (update != null)
            {
                if (trangThai == 5)
                {
                    foreach (var item in chitiethoadon)
                    {
                        var bienThe = reposBienThe.GetAll().FirstOrDefault(p => p.ID == item.IDBienThe);
                        bienThe.SoLuong += item.SoLuong;
                        reposBienThe.Update(bienThe);
                    }
                }
                if (trangThai == 6)
                {
                    update.NgayThanhToan = DateTime.Now;
                }
                update.TrangThaiGiaoHang = trangThai;
                update.IDNhanVien = idNhanVien;
                reposHoaDon.Update(update);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
