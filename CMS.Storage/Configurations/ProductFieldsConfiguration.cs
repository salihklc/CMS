using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class ProductFieldsConfiguration : IEntityTypeConfiguration<ProductFields>
    {
        public void Configure(EntityTypeBuilder<ProductFields> builder)
        {
            builder.ToTable("ProductFields", "product");
            builder.HasKey(r => r.Idx);          
            builder.Property(r => r.Idx).ValueGeneratedOnAdd();
        }
    }
}
