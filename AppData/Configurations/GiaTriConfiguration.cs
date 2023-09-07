using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class GiaTriConfiguration : IEntityTypeConfiguration<GiaTri>
    {
        public void Configure(EntityTypeBuilder<GiaTri> builder)
        {
            builder.ToTable("GiaTri");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(50)").IsRequired();
            builder.HasOne(x => x.ThuocTinh).WithMany(x => x.GiaTris).HasForeignKey(x => x.IdThuocTinh);
        }
    }
}
