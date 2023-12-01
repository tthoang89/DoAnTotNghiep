using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public  class VoucherView
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "mời bạn nhập Mã")]
        [StringLength(40, ErrorMessage = "Mã không được quá 40 kí tự")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "mời bạn chọn hình thức giảm giá ")]
        public int HinhThucGiamGia { get; set; }//0 là giảm theo %, 1 là giảm thẳng giá tiền
       
        public int SoTienCan { get; set; }
        
        public int GiaTri { get; set; }
        
        public DateTime NgayApDung { get; set; }
        
        public DateTime NgayKetThuc { get; set; }
        
        public int SoLuong { get; set; }
        [Required(ErrorMessage = "mời bạn nhập Mô Tả")]
        public string MoTa { get; set; }
        
        public int TrangThai { get; set; }
    }
}
