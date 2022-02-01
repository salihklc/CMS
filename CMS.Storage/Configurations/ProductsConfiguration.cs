using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasKey(r => r.Idx);
            builder.ToTable("Products", "product");
            builder.HasMany(r => r.ProductFields).WithOne(k => k.Products).HasForeignKey(m => m.ProductIdx);
        }
    }
}
