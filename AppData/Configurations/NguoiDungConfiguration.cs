using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class NguoiDungConfiguration : IEntityTypeConfiguration<NguoiDung>
    {
        public void Configure(EntityTypeBuilder<NguoiDung> builder)
        {
            builder.ToTable("NguoiDung");
            builder.HasKey(x => x.IDNguoiDung);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(15)");
            builder.Property(x => x.TenDem).HasColumnType("nvarchar(15)");
            builder.Property(x => x.Ho).HasColumnType("nvarchar(15)");
            builder.Property(x => x.Password).HasColumnType("varchar(15)");
            builder.Property(x => x.GioiTinh).HasColumnType("int");
            builder.Property(x => x.NgaySinh).HasColumnType("datetime");
            builder.Property(x => x.Email).HasColumnType("varchar(50)");
            builder.Property(x => x.DiaChi).HasColumnType("nvarchar(100)");
            builder.Property(x => x.SDT).HasColumnType("varchar(10)");
            builder.Property(x => x.DiemTich).HasColumnType("int");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.VaiTro).WithMany(x => x.NguoiDungs).HasForeignKey(x => x.IDVaiTro);
            builder.HasOne(x => x.GioHang).WithOne(x => x.NguoiDung).HasForeignKey<GioHang>();
        }
    }
}
