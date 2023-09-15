using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class ThuocTinhSanPhamConfiguration : IEntityTypeConfiguration<ThuocTinhSanPham>
    {
        public void Configure(EntityTypeBuilder<ThuocTinhSanPham> builder)   
        {
            builder.ToTable("ThuocTinhSanPham");
            builder.HasKey(x => x.ID);
            builder.HasOne(x => x.ThuocTinh).WithMany(x => x.ThuocTinhLoaiSPs).HasForeignKey(x => x.IDThuocTinh);
            builder.HasOne(x => x.SanPham).WithMany(x => x.ThuocTinhSanPhams).HasForeignKey(x => x.IDSanPham);
        }
    }
}
