using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class BienTheService : IBienTheService
    {
        private readonly AssignmentDBContext _context;
        public BienTheService()
        {
            this._context = new AssignmentDBContext();
        }
        #region BienThe
        //Đặt BT mặc định
        public void SetBienTheDefault(Guid idbt)
        {
            try
            {
                var bt = _context.BienThes.FirstOrDefault(c => c.ID == idbt);
                bt.IsDefault = true;
                _context.BienThes.Update(bt);
                _context.SaveChanges();
                var lstbt = _context.BienThes.Where(c => c.IDSanPham == bt.IDSanPham).ToList();
                var result = lstbt.Where(c => c.ID != bt.ID).ToList();
                foreach (var item in result)
                {
                    item.IsDefault = false;
                    _context.BienThes.Update(item);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //Xóa BT
        public async Task<bool> DeleteBienThe(Guid id)
        {
            var bienthe = await _context.BienThes.FindAsync(id);
            //Xóa Chi tiết biến thể
            var listctbt = await _context.ChiTietBienThes.AsNoTracking().Where(c => c.IDBienThe == id).ToListAsync();
            _context.ChiTietBienThes.RemoveRange(listctbt);
            //Xóa Ảnh Bt
            var listanhbt = await _context.AnhBienThes.AsNoTracking().Where(c => c.ID == id).ToListAsync();
            _context.AnhBienThes.RemoveRange(listanhbt);
            //Xóa BT
            _context.BienThes.Remove(bienthe);
            await _context.SaveChangesAsync();
            return true;
        }
        //Lấy BT theo ID
        public async Task<BienTheViewModel> GetBienTheById(Guid idBienThe)
        {
            if (!_context.BienThes.Any(c => c.ID == idBienThe)) throw new Exception($"Không tìm thấy Sản phẩm có Id: {idBienThe}");
            var bthe = await (from sp in _context.SanPhams
                              join bt in _context.BienThes on sp.ID equals bt.IDSanPham
                              where bt.ID == idBienThe
                              select new BienTheViewModel()
                              {
                                  ID = bt.ID,
                                  Ten = sp.Ten,
                                  SoLuong = bt.SoLuong,
                                  GiaGoc = bt.GiaBan,
                                  GiaBan = bt.IDKhuyenMai == null ? bt.GiaBan : (from kh in _context.KhuyenMais where bt.IDKhuyenMai == kh.ID select kh.GiaTri * bt.GiaBan).FirstOrDefault(),
                                  TrangThai = bt.TrangThai,
                                  Anh = (from img in _context.Anhs.AsNoTracking() join btimg in _context.AnhBienThes on img.ID equals btimg.IdAnh where btimg.IdBienThe == bt.ID select img.Ten).ToList(),
                                  GiaTris = (from gt in _context.GiaTris
                                             join ctbt in _context.ChiTietBienThes on gt.ID equals ctbt.IDGiaTri
                                             where ctbt.IDBienThe == bt.ID
                                             select new GiaTriRequest()
                                             {
                                                 ID = gt.ID,
                                                 Ten = gt.Ten
                                             }).ToList()
                              }).FirstOrDefaultAsync();
            return bthe;
        }
        //Lấy BT theo IDSP
        public async Task<List<BienTheViewModel>> GetBienTheByIdSanPham(Guid idProduct)
        {
            if (!_context.SanPhams.Any(c => c.ID == idProduct)) throw new Exception($"Không tìm thấy Sản phẩm có Id: {idProduct}");

            var query = await (from sp in _context.SanPhams.AsNoTracking()
                               join bt in _context.BienThes.AsNoTracking() on sp.ID equals bt.IDSanPham
                               where bt.IDSanPham == idProduct
                               select new BienTheViewModel()
                               {
                                   ID = bt.ID,
                                   Ten = sp.Ten,
                                   SoLuong = bt.SoLuong,
                                   GiaGoc = bt.GiaBan,
                                   GiaBan = bt.IDKhuyenMai == null ? bt.GiaBan : (from kh in _context.KhuyenMais where bt.IDKhuyenMai == kh.ID select kh.GiaTri * bt.GiaBan).FirstOrDefault(),
                                   TrangThai = bt.TrangThai,
                                   Anh = (from img in _context.Anhs.AsNoTracking() join btimg in _context.AnhBienThes on img.ID equals btimg.IdAnh where btimg.IdBienThe == bt.ID select img.Ten).ToList(),
                                   GiaTris = (from gt in _context.GiaTris
                                              join ctbt in _context.ChiTietBienThes on gt.ID equals ctbt.IDGiaTri
                                              where ctbt.IDBienThe == bt.ID
                                              select new GiaTriRequest()
                                              {
                                                  ID = gt.ID,
                                                  Ten = gt.Ten
                                              }).ToList()
                               }).ToListAsync();


            return query;
        }
        // Lấy BT thông qua list Giá trị
        public async Task<BienTheViewModel> GetBTByListGiaTri(BienTheTruyVan bttv)
        {
            var query = await (from ctbt in _context.ChiTietBienThes.AsNoTracking()
                               join bt in _context.BienThes.AsNoTracking() on ctbt.IDBienThe equals bt.ID
                               join sp in _context.SanPhams.AsNoTracking() on bt.IDSanPham equals sp.ID
                               where sp.ID == bttv.idSp
                               select ctbt).ToListAsync();

            var results = from p in query
                          group p.IDGiaTri by p.IDBienThe into g
                          select new { IdBienThe = g.Key, GiaTris = g.ToList() };
            Guid idbt = Guid.Empty;
            foreach (var item in results)
            {
                var areEquivalent = (item.GiaTris.Count() == bttv.lstIdGTri.Count()) && !item.GiaTris.Except(bttv.lstIdGTri).Any();
                if (areEquivalent == true)
                {
                    idbt = item.IdBienThe;
                    break;
                }

            }
            var bthe = await GetBienTheById(idbt);
            return bthe;
        }
        //Lưu BT
        public async Task<BienThe> SaveBienThe(BienTheRequest request)
        {
            // Check tồn tại
            var bienthe = await _context.BienThes.FindAsync(request.ID);
            if (bienthe != null) // Update
            {
                var sp = await _context.SanPhams.FindAsync(request.IDSanPham);
                if (sp.TrangThai == 0 || request.SoLuong <= 0)
                {
                    request.TrangThai = 0;
                }
                else { request.TrangThai = 1; };
                bienthe.SoLuong = request.SoLuong;
                bienthe.GiaBan = request.GiaBan;
                bienthe.TrangThai = request.TrangThai;
                bienthe.IDKhuyenMai = request.IDKhuyenMai;
                _context.BienThes.Update(bienthe);
                await _context.SaveChangesAsync();

                // Giá trị
                foreach (var id in request.ListIdGiaTri)
                {
                    // Biến thể chưa có giá trị này -> Tạo CTBT
                    if (!_context.ChiTietBienThes.AsNoTracking().Where(c => c.IDBienThe == bienthe.ID).Any(c => c.IDGiaTri == id))
                    {
                        //Tạo CTBT
                        ChiTietBienThe ctbt = new ChiTietBienThe()
                        {
                            IDBienThe = bienthe.ID,
                            IDGiaTri = id,
                            TrangThai = 1,
                        };
                        await _context.ChiTietBienThes.AddAsync(ctbt);
                        await _context.SaveChangesAsync();
                    }
                }
                //Xóa CTBT không còn SD
                var lstIdCTBT = await _context.ChiTietBienThes.AsNoTracking().Where(c => c.IDBienThe == bienthe.ID).OrderByDescending(c => c.NgayLuu).Select(c => c.ID).Take(request.ListIdGiaTri.Count).ToListAsync();
                var lstDelete = await _context.ChiTietBienThes.AsNoTracking().Where(c => !lstIdCTBT.Contains(c.ID) & c.IDBienThe == bienthe.ID).ToListAsync();
                _context.ChiTietBienThes.RemoveRange(lstDelete);
                await _context.SaveChangesAsync();
                //Anh
                foreach (var item in request.LstImagePath)
                {
                    var fileName = Path.GetFileName(item);
                    if (!CheckImageExist(fileName)) // -> chưa tồn tại trong wwwroot
                    {
                        // Lưu file
                        var imgName = SaveFile(item);// Lưu vào root
                                                     //Lưu ảnh
                        Anh anh = new Anh()
                        {
                            ID = new Guid(),
                            Ten = fileName,
                            TrangThai = 1,
                        };
                        await _context.AddAsync(anh);
                        await _context.SaveChangesAsync();
                        // Tạo BT-Anh
                        AnhBienThe bta = new AnhBienThe()
                        {
                            ID = new Guid(),
                            IdAnh = anh.ID,
                            IdBienThe = bienthe.ID,
                            NgayTao = DateTime.Now,
                        };
                        await _context.AnhBienThes.AddAsync(bta);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var idanh = await _context.Anhs.AsNoTracking().Where(c => c.Ten == fileName).FirstOrDefaultAsync();
                        // Check BT da có ảnh này chưa 
                        var exist = await _context.AnhBienThes.AsNoTracking().Where(c => c.IdAnh == idanh.ID && c.IdBienThe == bienthe.ID).FirstOrDefaultAsync();
                        if (exist == null)
                        {
                            // Tạo BT-Anh
                            AnhBienThe bta = new AnhBienThe()
                            {
                                ID = new Guid(),
                                IdAnh = idanh.ID,
                                IdBienThe = bienthe.ID,
                                NgayTao = DateTime.Now,
                            };
                            await _context.AnhBienThes.AddAsync(bta);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                //Xóa ảnh bthe ko cần
                var lstAnhSD = await _context.AnhBienThes.Where(c => c.IdBienThe == bienthe.ID).OrderByDescending(c => c.NgayTao).Select(c => c.ID).Take(request.LstImagePath.Count).ToListAsync();
                var lstAnhDelete = await _context.AnhBienThes.Where(c => !lstAnhSD.Contains(c.ID)).ToListAsync();
                _context.AnhBienThes.RemoveRange(lstAnhDelete);
                await _context.SaveChangesAsync();
                return bienthe;
            }
            else //Tạo mới
            {
                var sp = await _context.SanPhams.FindAsync(request.IDSanPham);
                if (sp.TrangThai == 0 || request.SoLuong <= 0)
                {
                    request.TrangThai = 0;
                }
                else { request.TrangThai = 1; };
                var btdefault = _context.BienThes.Any(c => c.IDSanPham == request.IDSanPham && c.IsDefault == true);
                BienThe bienThe = new BienThe();
                bienThe.ID = new Guid();
                bienThe.SoLuong = request.SoLuong;
                bienThe.NgayTao = DateTime.Now;
                bienThe.TrangThai = request.TrangThai;
                bienThe.GiaBan = request.GiaBan;
                bienThe.IsDefault = btdefault == true ? false : true ;
                bienThe.IDKhuyenMai = request.IDKhuyenMai;
                bienThe.IDSanPham = request.IDSanPham;
                await _context.BienThes.AddAsync(bienThe);
                await _context.SaveChangesAsync();

                // Tạo CTBT
                foreach (var id in request.ListIdGiaTri)
                {
                    ChiTietBienThe ctbt = new ChiTietBienThe()
                    {
                        ID = new Guid(),
                        IDGiaTri = id,
                        IDBienThe = bienThe.ID,
                        TrangThai = 1,
                    };
                    await _context.ChiTietBienThes.AddAsync(ctbt);
                    await _context.SaveChangesAsync();
                }
                //Anh
                foreach (var item in request.LstImagePath)
                {
                    var fileName = Path.GetFileName(item);
                    if (!CheckImageExist(fileName)) // -> chưa tồn tại trong wwwroot
                    {
                        // Lưu file
                        var imgName = SaveFile(item);// Lưu vào root
                                                     //Lưu ảnh
                        Anh anh = new Anh()
                        {
                            ID = new Guid(),
                            Ten = fileName,
                            TrangThai = 1,
                        };
                        await _context.AddAsync(anh);
                        await _context.SaveChangesAsync();
                        // Tạo BT-Anh
                        AnhBienThe bta = new AnhBienThe()
                        {
                            ID = new Guid(),
                            IdAnh = anh.ID,
                            IdBienThe = bienThe.ID,
                            NgayTao = DateTime.Now,
                        };
                        await _context.AnhBienThes.AddAsync(bta);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var idanh = await _context.Anhs.AsNoTracking().Where(c => c.Ten == fileName).FirstOrDefaultAsync();
                        // Check BT da có ảnh này chưa 
                        var exist = await _context.AnhBienThes.AsNoTracking().Where(c => c.IdAnh == idanh.ID && c.IdBienThe == bienThe.ID).FirstOrDefaultAsync();
                        if (exist == null)
                        {
                            // Tạo BT-Anh
                            AnhBienThe bta = new AnhBienThe()
                            {
                                ID = new Guid(),
                                IdAnh = idanh.ID,
                                IdBienThe = bienThe.ID,
                                NgayTao = DateTime.Now,
                            };
                            await _context.AnhBienThes.AddAsync(bta);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                return bienThe;
            };
        }
        #endregion
        #region Anh
        public bool CheckImageExist(string imgName)
        {
            string basePath = Directory.GetCurrentDirectory();
            string destinationPath = Path.Combine(Path.GetDirectoryName(basePath),"AppView", "wwwroot", "img", "variants");

            // Lấy danh sách tệp tin trong thư mục
            string[] files = Directory.GetFiles(destinationPath);

            // Kiểm tra sự tồn tại của tệp ảnh cụ thể trong danh sách tệp tin
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                if (fileName.Equals(imgName, StringComparison.OrdinalIgnoreCase))
                {
                    // Kiểm tra định dạng tệp tin có phải là ảnh hay không
                    string extension = Path.GetExtension(file);
                    string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Có thể mở rộng danh sách đuôi tệp tin ảnh tùy ý
                    if (imageExtensions.Contains(extension.ToLower()))
                    {
                        return true; // Tệp ảnh tồn tại trong thư mục
                    }
                }
            }
            return false; // Tệp ảnh không tồn tại trong thư mục hoặc không phải là tệp ảnh
        }

        public Task<bool> DeleteAnh(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<string> GetListImg(List<string> imagePaths)
        {
            List<string> results = imagePaths.Distinct().ToList();
            return results;
        }

        public async void SaveAnh(string imgPath, Guid idbt)
        {
            var fileName = Path.GetFileName(imgPath);
            if (!CheckImageExist(fileName)) // -> chưa tồn tại trong wwwroot
            {
                // Lưu file
                var imgName = SaveFile(imgPath);// Lưu vào root
                                                //Lưu ảnh
                Anh anh = new Anh()
                {
                    ID = new Guid(),
                    Ten = fileName,
                    TrangThai = 1,
                };
                await _context.AddAsync(anh);
                await _context.SaveChangesAsync();
                // Tạo BT-Anh
                AnhBienThe bta = new AnhBienThe()
                {
                    ID = new Guid(),
                    IdAnh = anh.ID,
                    IdBienThe = idbt,
                    NgayTao = DateTime.Now,
                };
                await _context.AnhBienThes.AddAsync(bta);
                await _context.SaveChangesAsync();
            }
            else
            {
                var idanh = await _context.Anhs.AsNoTracking().Where(c => c.Ten == fileName).FirstOrDefaultAsync();
                // Check BT da có ảnh này chưa 
                var exist = await _context.AnhBienThes.AsNoTracking().Where(c => c.IdAnh == idanh.ID && c.IdBienThe == idbt).FirstOrDefaultAsync();
                if (exist == null)
                {
                    // Tạo BT-Anh
                    AnhBienThe bta = new AnhBienThe()
                    {
                        ID = new Guid(),
                        IdAnh = idanh.ID,
                        IdBienThe = idbt,
                    };
                    await _context.AnhBienThes.AddAsync(bta);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<string> SaveFile(string imagePath)
        {
            // Kiểm tra xem tệp ảnh tồn tại hay không
            if (File.Exists(imagePath))
            {
                // Lấy tên tệp ảnh
                string imageName = Path.GetFileName(imagePath);

                // Tạo đường dẫn đến thư mục đích
                string basePath = Directory.GetCurrentDirectory();
                string destinationPath = Path.Combine(Path.GetDirectoryName(basePath), "AppView", "wwwroot", "img", "variants", imageName);

                // Di chuyển hoặc sao chép tệp ảnh vào thư mục đích
                File.Copy(imagePath, destinationPath, true);
                return imageName;
            }
            return null;
        }
        #endregion
    }

}
