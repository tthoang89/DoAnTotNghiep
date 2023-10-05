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
    public class PhuongThucThanhToanConfiguration : IEntityTypeConfiguration<PhuongThucThanhToan>
    {
        public void Configure(EntityTypeBuilder<PhuongThucThanhToan> builder)
        {
            builder.ToTable("PhuongThucThanhToan");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(50)");
            builder.Property(x => x.TrangThai).HasColumnType("int");
        }
    }
}
