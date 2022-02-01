using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class PrioritiesConfiguration : IEntityTypeConfiguration<Priorities>
    {
        public void Configure(EntityTypeBuilder<Priorities> builder)
        {
            builder.HasKey(r => r.Idx);
            builder.ToTable("Priorities");
            builder.HasMany(r => r.Tickets).WithOne(r => r.Priorities).HasForeignKey(k => k.PriorityIdx);
        }
    }
}
