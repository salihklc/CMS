using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class LabelsConfiguration : IEntityTypeConfiguration<Labels>
    {
        public void Configure(EntityTypeBuilder<Labels> builder)
        {
            builder.ToTable("Labels");
            builder.HasKey(r => r.Idx);
        }
    }
}
