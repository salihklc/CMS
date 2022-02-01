using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class DistrictsConfiguration : IEntityTypeConfiguration<Districts>
    {
        public void Configure(EntityTypeBuilder<Districts> builder)
        {
            builder.ToTable("Districts");
            builder.HasKey(e => e.Idx);
            builder.HasOne(c => c.City).WithMany(d => d.Districts).HasForeignKey(d => d.CityNo);
        }
    }
}
