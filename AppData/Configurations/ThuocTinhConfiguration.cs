using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class ThuocTinhConfiguration : IEntityTypeConfiguration<ThuocTinh>
    {
        public void Configure(EntityTypeBuilder<ThuocTinh> builder)
        {
            builder.ToTable("ThuocTinh");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(20)").IsRequired();
            builder.Property(x => x.NgayTao).HasColumnType("Datetime");
            builder.Property(x => x.TrangThai).HasColumnType("int");
        }
    }
}
