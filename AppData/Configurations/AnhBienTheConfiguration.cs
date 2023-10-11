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
    public class AnhBienTheConfiguration : IEntityTypeConfiguration<AnhBienThe>
    {
        public void Configure(EntityTypeBuilder<AnhBienThe> builder)
        {
            builder.ToTable("BienTheAnh");
            builder.HasKey(x => x.ID);
            builder.HasOne(x => x.BienThe).WithMany(x => x.BienTheAnhs).HasForeignKey(x => x.IdBienThe);
            builder.HasOne(x => x.Anh).WithMany(x => x.AnhBienThes).HasForeignKey(x => x.IdAnh);
        }
    }
}
