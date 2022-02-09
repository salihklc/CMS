using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class FirmProductFieldsConfiguraition : IEntityTypeConfiguration<FirmProductFields>
    {
        public void Configure(EntityTypeBuilder<FirmProductFields> builder)
        {
            builder.ToTable("FirmProductFields", "product");
            builder.HasKey(r => new { r.ProductFieldIdx, r.FirmProductIdx });
            builder.Property(k => k.Idx).ValueGeneratedOnAdd();
            builder.HasOne(r => r.ProductFields).WithMany(r => r.FirmProductFields).HasForeignKey(r => r.ProductFieldIdx);
            builder.HasOne(r => r.FirmProducts).WithMany(r => r.FirmProductFields).HasForeignKey(r => r.FirmProductIdx);
        }
    }
}
