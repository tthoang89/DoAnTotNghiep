using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Configurations
{
    public class ChiTietPTTTConfiguration : IEntityTypeConfiguration<ChiTietPTTT>
    {
        public void Configure(EntityTypeBuilder<ChiTietPTTT> builder)
        {
            builder.ToTable("ChiTietPTTT");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.SoTien).HasColumnType("int");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.HoaDon).WithMany(x => x.ChiTietPTTTs).HasForeignKey(x => x.IDHoaDon);
            builder.HasOne(x => x.PhuongThucThanhToan).WithMany(x => x.ChiTietPTTTs).HasForeignKey(x => x.IDPTTT);
        }
    }
}
