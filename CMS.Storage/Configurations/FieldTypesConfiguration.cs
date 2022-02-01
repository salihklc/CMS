using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class FieldTypesConfiguration : IEntityTypeConfiguration<FieldTypes>
    {
        public void Configure(EntityTypeBuilder<FieldTypes> builder)
        {
            builder.ToTable("FieldTypes", "product");
            builder.HasKey(r => r.Idx);
            builder.HasMany(k => k.Fields).WithOne(l => l.Types).HasForeignKey(m => m.TypeIdx);
        }
    }
}
