using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class FirmsConfiguration : IEntityTypeConfiguration<Firms>
    {
        public void Configure(EntityTypeBuilder<Firms> builder)
        {
            builder.ToTable("Firms");
            builder.HasKey(r => r.Idx);
            builder.HasMany(r => r.Tickets).WithOne(k => k.Firms).HasForeignKey(x => x.FirmIdx);
            builder.HasMany(r => r.Users).WithOne(k => k.Firms).HasForeignKey(x => x.FirmIdx);
        }
    }
}
