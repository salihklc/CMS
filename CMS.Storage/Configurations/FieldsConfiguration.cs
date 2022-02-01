using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class FieldsConfiguration : IEntityTypeConfiguration<Fields>
    {
        public void Configure(EntityTypeBuilder<Fields> builder)
        {
            builder.ToTable("Fields","product");
            builder.HasKey(r => r.Idx);
            builder.HasMany(r => r.ProductFields).WithOne(k => k.Fields).HasForeignKey(m => m.FieldIdx);
        }
    }
}
