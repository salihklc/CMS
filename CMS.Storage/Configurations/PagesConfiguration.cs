using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class PagesConfiguration : IEntityTypeConfiguration<Pages>
    {
        public void Configure(EntityTypeBuilder<Pages> builder)
        {
            builder.ToTable("Pages");
            builder.HasKey(x => x.Idx);
        }
    }
}
