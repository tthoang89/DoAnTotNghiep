using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class ThuocTinhLoaiSPConfiguration : IEntityTypeConfiguration<ThuocTinhLoaiSP>
    {
        public void Configure(EntityTypeBuilder<ThuocTinhLoaiSP> builder)   
        {
            builder.ToTable("ThuocTinhSanPham");
            builder.HasKey(x => x.ID);
            builder.HasOne(x => x.ThuocTinh).WithMany(x => x.ThuocTinhLoaiSPs).HasForeignKey(x => x.IDThuocTinh);
            builder.HasOne(x => x.LoaiSP).WithMany(x => x.ThuocTinhLoaiSPs).HasForeignKey(x => x.IDLoaiSP);
        }
    }
}
