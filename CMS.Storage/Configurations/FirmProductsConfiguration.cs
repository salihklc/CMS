using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class FirmProductsConfiguration : IEntityTypeConfiguration<FirmProducts>
    {
        public void Configure(EntityTypeBuilder<FirmProducts> builder)
        {
            builder.ToTable("FirmProducts", "product");
            builder.HasKey(r => r.Idx);
            builder.HasOne(r => r.Products).WithMany(r => r.FirmProducts).HasForeignKey(r => r.ProductIdx);
            builder.HasOne(r => r.Firms).WithMany(r => r.FirmProducts).HasForeignKey(r => r.FirmIdx);
        }
    }
}
