using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class ChiTietGioHangConfiguration : IEntityTypeConfiguration<ChiTietGioHang>
    {
        public void Configure(EntityTypeBuilder<ChiTietGioHang> builder)
        {
            builder.ToTable("ChiTietGioHang");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.SoLuong).HasColumnType("int").IsRequired();
            builder.HasOne(x => x.BienThe).WithMany(x => x.ChiTietGioHangs).HasForeignKey(x => x.IDBienThe);
            builder.HasOne(x => x.GioHang).WithMany(x => x.ChiTietGioHangs).HasForeignKey(x => x.IDNguoiDung);
        }
    }
}
